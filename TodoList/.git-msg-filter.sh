case "$GIT_COMMIT" in
60a0baf*)
  printf "%s\n" "lesson 2: update author names in Program and README"
  ;;
720f3fc*)
  printf "%s\n" "lesson 2: refactor Program command loop and clean up output"
  ;;
304dcce*)
  printf "%s\n" "refactor: split TodoList into separate classes and project files"
  ;;
2f05d3a*)
  printf "%s\n" "feat: add separate command classes (Help, Exit, Profile, Add, View, Done, Delete, Update, Read)"
  ;;
a1df00d*)
  printf "%s\n" "refactor: extract CommandParser from Program into a separate class"
  ;;
81de328*)
  printf "%s\n" "docs: update TodoListREADME with command descriptions and usage"
  ;;
f08631c*)
  printf "%s\n" "feat: add FileManager, TodoItem model and command implementations (Add, Done, Update, Delete, Profile)"
  ;;
0d30474*)
  printf "%s\n" "feat: integrate FileManager and profile support into TodoList program and README"
  ;;
5127416*)
  printf "%s\n" "refactor: update CommandParser and TodoList commands to use FileManager and new status logic"
  ;;
*)
  cat
  ;;
esac
