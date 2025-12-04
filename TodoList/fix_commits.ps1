# Полностью автоматическое переименование коммитов
# ВНИМАНИЕ: Это изменит историю коммитов!

Write-Host "=== Автоматическое переименование коммитов ===" -ForegroundColor Green
Write-Host ""

# Отключаем пейджер
$env:GIT_PAGER = ''
$env:GIT_EDITOR = 'powershell'

# Создаем скрипт для редактирования rebase-todo
$sequenceEditor = @'
param($file)
$content = Get-Content $file -Raw -Encoding UTF8
$content = $content -replace 'pick 5127416 .*', 'reword 5127416 Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events'
$content = $content -replace 'pick 0d30474 .*', 'reword 0d30474 Implement multi-user system with profile support'  
$content = $content -replace 'pick 304dcce .*', 'reword 304dcce Refactor TodoList to use classes (practical assignment lecture 6 OOP)'
Set-Content -Path $file -Value $content -Encoding UTF8 -NoNewline
'@

$seqEditorPath = "$PWD\temp_seq_editor.ps1"
$sequenceEditor | Out-File -FilePath $seqEditorPath -Encoding UTF8

# Создаем скрипт для редактирования сообщений коммитов
$commitEditor = @'
param($file)
$hash = $args[1]
$newMsg = switch ($hash) {
    '5127416' { 'Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events' }
    '0d30474' { 'Implement multi-user system with profile support' }
    '304dcce' { 'Refactor TodoList to use classes (practical assignment lecture 6 OOP)' }
    default { Get-Content $file -Raw }
}
Set-Content -Path $file -Value $newMsg -Encoding UTF8 -NoNewline
'@

$commitEditorPath = "$PWD\temp_commit_editor.ps1"
$commitEditor | Out-File -FilePath $commitEditorPath -Encoding UTF8

# Устанавливаем редакторы
$env:GIT_SEQUENCE_EDITOR = "powershell -File `"$seqEditorPath`" `$1"
$env:GIT_EDITOR = "powershell -File `"$commitEditorPath`" `$1 `$2"

Write-Host "Запускаю rebase..." -ForegroundColor Yellow

try {
    git rebase -i HEAD~15
    
    Write-Host ""
    Write-Host "Rebase завершен!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Выполните для отправки:" -ForegroundColor Yellow
    Write-Host "  git push --force-with-lease origin feature/todolist" -ForegroundColor Cyan
}
catch {
    Write-Host "Ошибка: $_" -ForegroundColor Red
}
finally {
    Remove-Item $seqEditorPath -ErrorAction SilentlyContinue
    Remove-Item $commitEditorPath -ErrorAction SilentlyContinue
}

