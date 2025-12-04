# Автоматическое переименование коммитов через git rebase
# ВНИМАНИЕ: Это изменит историю коммитов!

Write-Host "Автоматическое переименование коммитов" -ForegroundColor Green
Write-Host ""

# Отключаем пейджер
$env:GIT_PAGER = ''

# Создаем временный скрипт для редактирования rebase-todo
$editorScript = @'
$file = $args[0]
$content = Get-Content $file -Raw -Encoding UTF8

# Заменяем pick на reword для проблемных коммитов
$content = $content -replace 'pick 5127416 .*', 'reword 5127416 Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events'
$content = $content -replace 'pick 0d30474 .*', 'reword 0d30474 Implement multi-user system with profile support'
$content = $content -replace 'pick 304dcce .*', 'reword 304dcce Refactor TodoList to use classes (practical assignment lecture 6 OOP)'

Set-Content -Path $file -Value $content -Encoding UTF8 -NoNewline
'@

$editorScriptPath = Join-Path $PSScriptRoot "temp_rebase_editor.ps1"
$editorScript | Out-File -FilePath $editorScriptPath -Encoding UTF8

# Устанавливаем редактор
$env:GIT_SEQUENCE_EDITOR = "powershell -File `"$editorScriptPath`""

Write-Host "Запускаю интерактивный rebase для последних 15 коммитов..." -ForegroundColor Yellow
Write-Host "Это может занять некоторое время..." -ForegroundColor Yellow
Write-Host ""

try {
    # Запускаем rebase
    git rebase -i HEAD~15
    
    Write-Host ""
    Write-Host "Rebase завершен успешно!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Теперь нужно отправить изменения:" -ForegroundColor Yellow
    Write-Host "git push --force-with-lease origin feature/todolist" -ForegroundColor Cyan
}
catch {
    Write-Host "Ошибка при выполнении rebase: $_" -ForegroundColor Red
}
finally {
    # Удаляем временный скрипт
    if (Test-Path $editorScriptPath) {
        Remove-Item $editorScriptPath -Force
    }
}

