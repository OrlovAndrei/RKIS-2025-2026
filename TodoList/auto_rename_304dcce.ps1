# Автоматическое переименование коммита 304dcce
$env:GIT_EDITOR = 'powershell -Command "Set-Content -Path `$args[0] -Value ''Refactor TodoList to use classes (lecture 6 OOP practical assignment)'' -Encoding UTF8"'
$env:GIT_SEQUENCE_EDITOR = 'powershell -Command "$content = Get-Content `$args[0] -Raw; $content = $content -replace ''^pick 304dcce'', ''reword 304dcce''; Set-Content -Path `$args[0] -Value $content -Encoding UTF8"'

git rebase -i 304dcce^

