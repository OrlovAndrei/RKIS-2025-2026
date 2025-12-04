# Скрипт для переименования всех коммитов от 1 декабря 2025
# Этот скрипт использует git filter-branch для переименования коммитов

Write-Host "=== Переименование коммитов от 1 декабря 2025 ===" -ForegroundColor Green
Write-Host ""

# Словарь для переименования коммитов (старое сообщение -> новое сообщение)
$commitRenames = @{
    "ефакторинг: использование делегатов в CommandParser и разделение TodoList/FileManager через события" = "Refactoring: use delegates in CommandParser and separate TodoList/FileManager via events"
    "еализована многопользовательская система с поддержкой нескольких профилей" = "Implement multi-user system with profile support"
    "РџРµСЂРµРїРёСЃР°Р» РєРѕРґ TodoList РїРѕРґ РєР»Р°СЃСЃС‹ (РїСЂР°РєС‚РёС‡РµСЃРєРѕРµ Р·Р°РґР°РЅРёРµ Р»РµРєС†РёСЏ 6 РћРћРџ)" = "Refactor TodoList to use classes (practical assignment lecture 6 OOP)"
    "Р<U+0098>РЅС‚РµРіСЂРёСЂРѕРІР°РЅРѕ Р°РІС‚РѕРјР°С‚РёС‡РµСЃРєРѕРµ СЃРѕС…СЂР°РЅРµРЅРёРµ РІ РєРѕРјР°РЅРґС‹ Add, Done, Update, Delete, Profile" = "Integrate automatic saving into Add, Done, Update, Delete, Profile commands"
    "РћР±РЅРѕРІР»РµРЅ README.md СЃ РѕРїРёСЃР°РЅРёРµРј РїР°С‚С‚РµСЂРЅР° Command Рё РЅРѕРІС‹С… РІРѕР·РјРѕР¶РЅРѕСЃС‚РµР№" = "Update README.md with Command pattern description and new features"
    "РџРµСЂРµСЂР°Р±РѕС‚Р°РЅ РіР»Р°РІРЅС‹Р№ С†РёРєР» РїСЂРѕРіСЂР°РјРјС‹ РґР»СЏ РёСЃРїРѕР»СЊР·РѕРІР°РЅРёСЏ РїР°С‚С‚РµСЂРЅР° Command" = "Refactor main program loop to use Command pattern"
    "Р РµР°Р»РёР·РѕРІР°РЅС‹ РєР»Р°СЃСЃС‹ РєРѕРјР°РЅРґ: HelpCommand, ExitCommand, ProfileCommand, AddCommand, ViewCommand, DoneCommand, DeleteCommand, UpdateCommand, ReadCommand" = "Implement command classes: HelpCommand, ExitCommand, ProfileCommand, AddCommand, ViewCommand, DoneCommand, DeleteCommand, UpdateCommand, ReadCommand"
}

Write-Host "ВНИМАНИЕ: Это изменит историю коммитов!" -ForegroundColor Red
Write-Host "Убедитесь, что у вас нет незакоммиченных изменений." -ForegroundColor Yellow
Write-Host ""
$confirm = Read-Host "Продолжить? (y/n)"

if ($confirm -ne "y") {
    Write-Host "Отменено." -ForegroundColor Yellow
    exit
}

# Используем git rebase для переименования
# Найдем первый коммит от 1 декабря
$firstCommit = git log --since="2025-12-01" --until="2025-12-02" --pretty=format:"%H" --reverse | Select-Object -First 1

if (-not $firstCommit) {
    Write-Host "Не найдено коммитов от 1 декабря 2025" -ForegroundColor Red
    exit
}

Write-Host "Найден первый коммит от 1 декабря: $firstCommit" -ForegroundColor Cyan

# Получаем родителя первого коммита для rebase
$baseCommit = git rev-parse "$firstCommit^"

Write-Host "Начинаю интерактивный rebase от коммита $baseCommit..." -ForegroundColor Yellow
Write-Host ""
Write-Host "В открывшемся редакторе:" -ForegroundColor Cyan
Write-Host "1. Найдите коммиты с проблемными названиями" -ForegroundColor White
Write-Host "2. Замените 'pick' на 'reword' (или 'r') для этих коммитов" -ForegroundColor White
Write-Host "3. Сохраните и закройте редактор" -ForegroundColor White
Write-Host "4. Для каждого коммита с 'reword' введите новое сообщение" -ForegroundColor White
Write-Host ""

# Запускаем интерактивный rebase
git rebase -i $baseCommit

Write-Host ""
Write-Host "Rebase завершен!" -ForegroundColor Green
Write-Host ""
Write-Host "Для отправки изменений выполните:" -ForegroundColor Yellow
Write-Host "  git push --force-with-lease origin feature/todolist" -ForegroundColor Cyan

