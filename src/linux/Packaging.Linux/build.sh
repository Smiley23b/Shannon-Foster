#!/bin/bash
die () {
    echo "$*" >&2
    exit 1
}

echo "Building Packaging.Linux..."

# Directories
THISDIR="$( cd "$(dirname "$0")" ; pwd -P )"
ROOT="$( cd "$THISDIR"/../../.. ; pwd -P )"
SRC="$ROOT/src"
OUT="$ROOT/out"
INSTALLER_SRC="$SRC/linux/Packaging.Linux"
INSTALLER_OUT="$OUT/linux/Packaging.Linux"

# Parse script arguments
for i in "$@"
do
case "$i" in
    --configuration=*)
    CONFIGURATION="${i#*=}"
    shift # past argument=value
    ;;
    --version=*)
    VERSION="${i#*=}"
    shift # past argument=value
    ;;
    --install-from-source=*)
    INSTALL_FROM_SOURCE="${i#*=}"
    shift # past argument=value
    ;;
    *)
          # unknown option
    ;;
esac
done

# Perform pre-execution checks
CONFIGURATION="${CONFIGURATION:=Debug}"
if [ -z "$VERSION" ]; then
    die "--version was not set"
fi

OUTDIR="$INSTALLER_OUT/$CONFIGURATION"
PAYLOAD="$OUTDIR/payload"

# Lay out payload
"$INSTALLER_SRC/layout.sh" --configuration="$CONFIGURATION" || exit 1

if [ $INSTALL_FROM_SOURCE = true ]; then
    INSTALL_LOCATION="/usr/local"
    mkdir -p "$INSTALL_LOCATION"

    echo "Installing..."

    # Install directories
    INSTALL_TO="$INSTALL_LOCATION/share/gcm-core/"
    LINK_TO="$INSTALL_LOCATION/bin/"

    mkdir -p "$INSTALL_TO" "$LINK_TO"

    # Copy all binaries and shared libraries to target installation location
    cp -R "$PAYLOAD"/* "$INSTALL_TO" || exit 1

    # Create symlink
    if [ ! -f "$LINK_TO/git-credential-manager" ]; then
        ln -s -r "$INSTALL_TO/git-credential-manager" \
            "$LINK_TO/git-credential-manager" || exit 1
    fi

    # Create legacy symlink with older name
    if [ ! -f "$LINK_TO/git-credential-manager-core" ]; then
        ln -s -r "$INSTALL_TO/git-credential-manager" \
            "$LINK_TO/git-credential-manager-core" || exit 1
    fi

    echo "Install complete."
else
    # Pack
    "$INSTALLER_SRC/pack.sh" --configuration="$CONFIGURATION" --payload="$PAYLOAD" --version="$VERSION" || exit 1
fi

echo "Build of Packaging.Linux complete."
