#! /bin/bash

if [ "$USER" != "root" ]; then
    echo "Wait, you are not root, fuck you!";
    exit 1;
fi

appname=ShevricTodo
bindir=/usr/local/bin
libdir="/usr/local/lib/$appname"

echo "Uninstalling program...";
if [ -h "$bindir/$appname" ]; then
	echo "Symlink found: unlinking...";
	unlink $bindir/$appname;
fi
if [ -d $libdir/ ]; then
	echo "Program lib directory found: deleting...";
	rm -r $libdir;
fi
echo "Program was sucessfully uninstalled from your computer!";
