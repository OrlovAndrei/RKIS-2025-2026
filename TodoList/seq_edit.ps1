param($file)
$content = Get-Content $file -Raw -Encoding UTF8
$content = $content -replace '^pick 304dcce', 'reword 304dcce'
Set-Content -Path $file -Value $content -Encoding UTF8 -NoNewline

