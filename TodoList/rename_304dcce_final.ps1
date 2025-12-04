# Переименование коммита 304dcce
# Новое название: "Refactor TodoList to use classes (lecture 6 OOP practical assignment)"

Write-Host "Переименование коммита 304dcce..." -ForegroundColor Green
Write-Host "Новое название: Refactor TodoList to use classes (lecture 6 OOP practical assignment)" -ForegroundColor Cyan

# Создаем скрипты для автоматического редактирования
$seqScript = @'
$file = $args[0]
$content = Get-Content $file -Raw -Encoding UTF8
$content = $content -replace '^pick 304dcce', 'reword 304dcce'
Set-Content -Path $file -Value $content -Encoding UTF8 -NoNewline
'@ | Out-File -FilePath "temp_seq.ps1" -Encoding UTF8

$commitScript = @'
$file = $args[0]
Set-Content -Path $file -Value "Refactor TodoList to use classes (lecture 6 OOP practical assignment)" -Encoding UTF8 -NoNewline
'@ | Out-File -FilePath "temp_commit.ps1" -Encoding UTF8

# Устанавливаем редакторы
$env:GIT_SEQUENCE_EDITOR = "powershell -File `"$PWD\temp_seq.ps1`" `$1"
$env:GIT_EDITOR = "powershell -File `"$PWD\temp_commit.ps1`" `$1"

Write-Host "Запускаю rebase..." -ForegroundColor Yellow
git rebase -i 304dcce^

# Очистка
Remove-Item "temp_seq.ps1" -ErrorAction SilentlyContinue
Remove-Item "temp_commit.ps1" -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "Готово! Теперь выполните:" -ForegroundColor Green
Write-Host "git push --force-with-lease origin feature/todolist" -ForegroundColor Cyan

