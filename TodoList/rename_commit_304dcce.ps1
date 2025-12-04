# Переименование коммита 304dcce
# На основе изменений: рефакторинг кода под классы (практическое задание лекция 6 ООП)

Write-Host "=== Переименование коммита 304dcce ===" -ForegroundColor Green
Write-Host ""
Write-Host "Текущее название (с неправильной кодировкой):" -ForegroundColor Yellow
Write-Host "РџРµСЂРµРїРёСЃР°Р» РєРѕРґ TodoList РїРѕРґ РєР»Р°СЃСЃС‹ (РїСЂР°РєС‚РёС‡РµСЃРєРѕРµ Р·Р°РґР°РЅРёРµ Р»РµРєС†РёСЏ 6 РћРћРџ)" -ForegroundColor Red
Write-Host ""
Write-Host "Новое название:" -ForegroundColor Yellow
Write-Host "Refactor TodoList to use classes (lecture 6 OOP practical assignment)" -ForegroundColor Green
Write-Host ""
Write-Host "Изменения в коммите:" -ForegroundColor Cyan
Write-Host "  - Удален Program.cs (260 строк)" -ForegroundColor White
Write-Host "  - Добавлен TodoListProfile.cs (46 строк)" -ForegroundColor White
Write-Host "  - Рефакторинг кода под классы" -ForegroundColor White
Write-Host ""

$confirm = Read-Host "Переименовать коммит? (y/n)"

if ($confirm -ne "y") {
    Write-Host "Отменено." -ForegroundColor Yellow
    exit
}

Write-Host ""
Write-Host "Начинаю интерактивный rebase..." -ForegroundColor Yellow
Write-Host ""

# Находим родителя коммита 304dcce
$parentCommit = git rev-parse "304dcce^"

Write-Host "Родительский коммит: $parentCommit" -ForegroundColor Cyan
Write-Host ""
Write-Host "Запускаю rebase от родителя коммита 304dcce..." -ForegroundColor Yellow
Write-Host ""

# Запускаем интерактивный rebase
# Нужно найти, сколько коммитов назад находится 304dcce
$commitsAhead = (git rev-list --count HEAD ^304dcce)
$totalCommits = (git rev-list --count HEAD)

Write-Host "Коммит 304dcce находится примерно на позиции $commitsAhead от HEAD" -ForegroundColor Cyan
Write-Host "Запускаю rebase для последних $($totalCommits - $commitsAhead + 5) коммитов..." -ForegroundColor Yellow
Write-Host ""

# Запускаем интерактивный rebase
git rebase -i "304dcce^"

Write-Host ""
Write-Host "После завершения rebase выполните:" -ForegroundColor Yellow
Write-Host "  git push --force-with-lease origin feature/todolist" -ForegroundColor Cyan

