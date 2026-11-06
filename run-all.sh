#!/usr/bin/env bash
# Runs every lesson, project and test in this repository and reports what failed.
# Usage: ./run-all.sh            (from the repository root; needs the .NET 10 SDK)
#        ./run-all.sh chapter-03  (only paths that contain "chapter-03")
set -uo pipefail
cd "$(dirname "$0")"
filter="${1:-}"
passed=0; nfail=0; failed=(); log="$(mktemp)"

# Stop any single step after 10 minutes. macOS has no "timeout" unless coreutils is installed.
if command -v timeout > /dev/null; then limit=(timeout 600)
elif command -v gtimeout > /dev/null; then limit=(gtimeout 600)
else limit=(); fi

run() {   # run <label> <command...>
  local label="$1"; shift
  [[ -n "$filter" && "$label" != *"$filter"* ]] && return
  printf '%-62s' "$label"
  if ${limit[@]+"${limit[@]}"} "$@" > "$log" 2>&1; then
    echo "ok"; passed=$((passed + 1))
  else
    echo "FAILED"; nfail=$((nfail + 1)); failed+=("$label"); sed 's/^/    /' "$log" | tail -20
  fi
}

# Some lessons read from the keyboard; feed them sample answers.
input_for() {
  case "$1" in
    *01-basics/08-console-io.cs)       printf 'Sam\n31\n' ;;
    *02-control-flow/04-while-loop.cs) printf 'milk\nbread\n\n' ;;
    *02-control-flow/05-do-while.cs)   printf 'abc\n0\n7\n' ;;
    *) : ;;
  esac
}

export -f input_for

# 1. Every single-file lesson.
for file in chapter-0*/*.cs; do
  case "$file" in
    *06-minimal-api.cs) run "$file" dotnet run "$file" -- --self-test ;;
    *) run "$file" bash -c 'input_for "$1" | dotnet run "$1"' _ "$file" ;;
  esac
done


# 2. Chapter 7 lessons that are whole projects or scripts.
run "chapter-07 04-dotnet-cli.sh"        bash chapter-07-modules-tooling/04-dotnet-cli.sh
run "chapter-07 03-multi-file-app"       dotnet run --project chapter-07-modules-tooling/03-multi-file-app
run "chapter-07 05-project-file"         dotnet run --project chapter-07-modules-tooling/05-project-file
run "chapter-07 07-class-library"        dotnet run --project chapter-07-modules-tooling/07-class-library/Storefront
run "chapter-07 08-unit-testing (tests)" dotnet test chapter-07-modules-tooling/08-unit-testing/Discounts.Tests

# 3. The nine projects, with their sample input.
for dir in projects/0[1-6]-* projects/08-*; do
  args=()
  [[ "$dir" == *02-number-guessing-game ]] && args=(-- 42)
  [[ "$dir" == *06-expense-tracker ]] && args=(-- "$(mktemp -d)")
  if [[ -f "$dir/sample-input.txt" ]]; then
    run "$dir" bash -c 'd="$1"; shift; dotnet run --project "$d" "$@" < "$d/sample-input.txt"' _ "$dir" ${args[@]+"${args[@]}"}
  else
    run "$dir" dotnet run --project "$dir" ${args[@]+"${args[@]}"}
  fi
done
run "projects/07-text-toolkit (tests)" dotnet test projects/07-text-toolkit/TextToolkit.slnx
run "projects/07-text-toolkit (cli)"   dotnet run --project projects/07-text-toolkit/src/TextToolkit.Cli -- projects/07-text-toolkit/samples/story.txt projects/07-text-toolkit/samples/terms.txt
run "projects/09-notes-api (tests)"    dotnet test projects/09-notes-api/NotesApi.slnx
run "projects/09-notes-api (demo)"     bash projects/09-notes-api/demo.sh

rm -f "$log"
echo
echo "Passed: $passed, failed: $nfail"
for f in ${failed[@]+"${failed[@]}"}; do echo "  - $f"; done
[[ $nfail -eq 0 ]]
