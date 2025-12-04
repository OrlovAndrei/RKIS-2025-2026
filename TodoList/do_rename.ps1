# Автоматическое переименование коммита 304dcce
Write-Host "Переименование коммита 304dcce..." -ForegroundColor Green

# Создаем скрипт для автоматического редактирования rebase-todo
$seqEditor = @'
param($file)
$content = Get-Content $file -Raw -Encoding UTF8
$content = $content -replace '^pick 304dcce', 'reword 304dcce'
Set-Content -Path $file -Value $content -Encoding UTF8 -NoNewline
'@

$seqEditorPath = "$PWD\temp_seq.ps1"
$seqEditor | Out-File -FilePath $seqEditorPath -Encoding UTF8

# Создаем скрипт для автоматического редактирования сообщения коммита
$commitEditor = @'
param($file)
Set-Content -Path $file -Value "Refactor TodoList to use classes (lecture 6 OOP practical assignment)" -Encoding UTF8 -NoNewline
'@

$commitEditorPath = "$PWD\temp_commit.ps1"
$commitEditor | Out-File -FilePath $commitEditorPath -Encoding UTF8

# Устанавливаем редакторы
$env:GIT_SEQUENCE_EDITOR = "powershell -File `"$seqEditorPath`" `$1"
$env:GIT_EDITOR = "powershell -File `"$commitEditorPath`" `$1"

Write-Host "Запускаю rebase..." -ForegroundColor Yellow
git rebase -i 304dcce^

# Очистка
Remove-Item $seqEditorPath -ErrorAction SilentlyContinue
Remove-Item $commitEditorPath -ErrorAction SilentlyContinue

Write-Host "Готово! Теперь выполните: git push --force-with-lease origin feature/todolist" -ForegroundColor Green

