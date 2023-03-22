#!/bin/bash

VERSION="1.0.0"
TEMP_NAME="Release"

function release() {
  TARGET=$1
  RELEASE_NAME="scrapper-${VERSION}-${TARGET}"
  dotnet publish -c Release --os $TARGET -o $TEMP_NAME
  cd $TEMP_NAME || exit 1
  if [ "$TARGET" == "win" ]; then
    7z a -tzip "../${RELEASE_NAME}.zip" *
  else
    tar czvf "../${RELEASE_NAME}.tar.gz" *
  fi
  cd .. || exit
}

mkdir $TEMP_NAME
release linux
release win
release osx
rm -r $TEMP_NAME
