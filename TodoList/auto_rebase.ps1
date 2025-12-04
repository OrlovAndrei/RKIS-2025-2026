# Автоматический rebase для переименования коммитов
# Этот скрипт использует git rebase с автоматическим редактированием

# Устанавливаем редактор для автоматического редактирования
$env:GIT_SEQUENCE_EDITOR = "powershell -Command `"`$input | ForEach-Object { if (`$_ -match '^(pick|reword) ([a-f0-9]+) (.*)') { `$hash = `$matches[2]; `$msg = `$matches[3]; if (`$hash -eq '5127416') { 'reword ' + `$hash + ' Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events' } elseif (`$hash -eq '0d30474') { 'reword ' + `$hash + ' Implement multi-user system with profile support' } elseif (`$hash -eq '304dcce') { 'reword ' + `$hash + ' Refactor TodoList to use classes (practical assignment lecture 6 OOP)' } else { `$_ } } else { `$_ } } | Out-File -FilePath `$args[0] -Encoding utf8`""

Write-Host "Начинаю интерактивный rebase..."
Write-Host "ВНИМАНИЕ: Это изменит историю коммитов!"
Write-Host "Нажмите Enter для продолжения или Ctrl+C для отмены"
Read-Host

# Запускаем rebase для последних 15 коммитов
git rebase -i HEAD~15

# После rebase нужно будет сделать force push
Write-Host ""
Write-Host "После успешного rebase выполните:"
Write-Host "git push --force-with-lease origin feature/todolist"

