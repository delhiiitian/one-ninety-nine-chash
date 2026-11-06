#!/usr/bin/env bash
# 7.4 The dotnet command-line interface
# Run: bash chapter-07-modules-tooling/04-dotnet-cli.sh
# (On Windows, run the same dotnet commands one by one in PowerShell.)
set -euo pipefail

work="$(mktemp -d)"
cd "$work"
echo "Working in $work"

echo "== dotnet --version: which SDK is installed"
dotnet --version

echo "== dotnet new console: create a project from a template"
dotnet new console --name HelloCli --output HelloCli > /dev/null
ls HelloCli

echo "== dotnet build: compile (Debug by default)"
dotnet build HelloCli --nologo -v quiet | tail -3

echo "== dotnet run: build if needed, then run"
dotnet run --project HelloCli

echo "== dotnet run with arguments after --"
cat > HelloCli/Program.cs <<'CS'
Console.WriteLine(args.Length == 0 ? "No arguments" : $"Hello, {string.Join(" and ", args)}!");
CS
dotnet run --project HelloCli -- Asha Ben

echo "== dotnet publish: produce a folder you can copy to another machine"
dotnet publish HelloCli -c Release -o out --nologo -v quiet > /dev/null
ls out | grep -i hellocli

echo "== dotnet new list: some available templates"
dotnet new list 2>/dev/null | grep -E "console|classlib|xunit|webapi" | head -5 || true

cd / && rm -rf "$work"
echo "Done."
