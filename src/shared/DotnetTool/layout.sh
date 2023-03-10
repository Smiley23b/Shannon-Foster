#!/bin/bash
make_absolute () {
    case "$1" in
    /*)
        echo "$1"
        ;;
    *)
        echo "$PWD/$1"
        ;;
    esac
}

#####################################################################
# Lay out
#####################################################################
# Parse script arguments
for i in "$@"
do
case "$i" in
    --configuration=*)
    CONFIGURATION="${i#*=}"
    shift # past argument=value
    ;;
    *)
          # unknown option
    ;;
esac
done

# Directories
THISDIR="$( cd "$(dirname "$0")" ; pwd -P )"
ROOT="$( cd "$THISDIR"/../../.. ; pwd -P )"
SRC="$ROOT/src"
OUT="$ROOT/out"
GCM_SRC="$SRC/shared/Git-Credential-Manager"
GCM_UI_SRC="$SRC/shared/Git-Credential-Manager.UI.Avalonia"
BITBUCKET_UI_SRC="$SRC/shared/Atlassian.Bitbucket.UI.Avalonia"
GITHUB_UI_SRC="$SRC/shared/GitHub.UI.Avalonia"
GITLAB_UI_SRC="$SRC/shared/GitLab.UI.Avalonia"
DOTNET_TOOL="shared/DotnetTool"
PROJ_OUT="$OUT/$DOTNET_TOOL"

CONFIGURATION="${CONFIGURATION:=Debug}"

# Build parameters
FRAMEWORK=net6.0

# Outputs
OUTDIR="$PROJ_OUT/nupkg/$CONFIGURATION"
IMGOUT="$OUTDIR/images"
PAYLOAD="$OUTDIR/payload"
SYMBOLOUT="$OUTDIR/payload.sym"

# Cleanup output directory
if [ -d "$OUTDIR" ]; then
    echo "Cleaning existing output directory '$OUTDIR'..."
    rm -rf "$OUTDIR"
fi

# Ensure output directories exist
mkdir -p "$PAYLOAD" "$SYMBOLOUT" "$IMGOUT"

if [ -z "$DOTNET_ROOT" ]; then
    DOTNET_ROOT="$(dirname $(which dotnet))"
fi

# Publish core application executables
echo "Publishing core application..."
$DOTNET_ROOT/dotnet publish "$GCM_SRC" \
    --configuration="$CONFIGURATION" \
    --framework="$FRAMEWORK" \
    --output="$(make_absolute "$PAYLOAD")" \
    -p:UseAppHost=false || exit 1

echo "Publishing core UI helper..."
$DOTNET_ROOT/dotnet publish "$GCM_UI_SRC" \
    --configuration="$CONFIGURATION" \
    --framework="$FRAMEWORK" \
    --output="$(make_absolute "$PAYLOAD")" \
    -p:UseAppHost=false || exit 1

echo "Publishing Bitbucket UI helper..."
$DOTNET_ROOT/dotnet publish "$BITBUCKET_UI_SRC" \
    --configuration="$CONFIGURATION" \
    --framework="$FRAMEWORK" \
    --output="$(make_absolute "$PAYLOAD")" \
    -p:UseAppHost=false || exit 1

echo "Publishing GitHub UI helper..."
$DOTNET_ROOT/dotnet publish "$GITHUB_UI_SRC" \
    --configuration="$CONFIGURATION" \
    --framework="$FRAMEWORK" \
    --output="$(make_absolute "$PAYLOAD")" \
    -p:UseAppHost=false || exit 1

echo "Publishing GitLab UI helper..."
$DOTNET_ROOT/dotnet publish "$GITLAB_UI_SRC" \
    --configuration="$CONFIGURATION" \
    --framework="$FRAMEWORK" \
    --output="$(make_absolute "$PAYLOAD")" \
    -p:UseAppHost=false || exit 1

# Collect symbols
echo "Collecting managed symbols..."
mv "$PAYLOAD"/*.pdb "$SYMBOLOUT" || exit 1

# Copy DotnetToolSettings.xml file
echo "Copying out package configuration files..."
cp "$SRC/$DOTNET_TOOL/DotnetToolSettings.xml" "$PAYLOAD/"

# Copy package icon image
echo "Copying images..."
cp "$SRC/$DOTNET_TOOL/icon.png" "$IMGOUT" || exit 1

echo "Build complete."
