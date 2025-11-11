#! /bin/bash
if [ "$USER" != "root" ]; then
    echo "Wait, you are not root, fuck you!";
    exit 1;
fi
echo "I am gay";

projdir=$PWD;
outdir="$projdir/build"
appname=shevricTodo
bindir=/usr/local/bin
libdir="/usr/local/lib/$appname"

if [ ! -f $projdir/shevricTodo.csproj ]; then
	echo "You should be in the project directory to run this script, fuck you!";
	exit 1;
fi

if [ ! -d "$projdir/build" ]; then
    echo "creating build directory...";
    mkdir build;
else echo "build directory already exists.";
fi

echo "cd into build directory...";
cd build;

echo "Starting compiler...";
dotnet publish $projdir -o $outdir;
if [ $? != 0 ]; then
	echo "Compilation failed, aborting...";
	exit 1;
fi
echo "Compilation succesfull.";

echo "installing program...";
if [ ! -d $libdir ]; then
	echo "creating lib directory...";
	mkdir $libdir;
else echo "lib directory already exists.";
fi
if [ -h $bindir/$appname ]; then
	echo "link exists, unlinking...";
	unlink $bindir/$appname;
fi
cp -rf . $libdir;
ln -s $libdir/$appname $bindir/;
echo "Program was installed succesfully! You can run it by typing $appname in the terminal.";
