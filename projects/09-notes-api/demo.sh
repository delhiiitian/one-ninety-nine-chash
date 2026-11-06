#!/usr/bin/env bash
# Starts the Notes API, sends a few requests with curl, then stops it.
# Run from this folder: ./demo.sh
set -euo pipefail
cd "$(dirname "$0")"
DB="$(mktemp -d)/demo-notes.db"
URL=http://localhost:5199

dotnet run --project src/NotesApi --no-launch-profile --urls "$URL" -- --ConnectionStrings:Notes="Data Source=$DB" > /dev/null &
SERVER=$!
trap 'kill $SERVER 2>/dev/null || true' EXIT

# Wait until the server answers.
for _ in $(seq 1 60); do curl -sf "$URL/health" > /dev/null && break; sleep 1; done

show() { echo; echo "\$ $*"; "$@"; echo; }
show curl -s "$URL/health"
show curl -s -X POST "$URL/notes" -H "Content-Type: application/json" -d '{"title":"Buy milk","body":"2 litres","tags":["home","shopping"]}'
show curl -s -X POST "$URL/notes" -H "Content-Type: application/json" -d '{"title":"Book review","body":"Chapter 9 draft","tags":["work"],"pinned":true}'
show curl -s "$URL/notes"
show curl -s "$URL/notes?tag=home"
show curl -s -X PUT "$URL/notes/1" -H "Content-Type: application/json" -d '{"title":"Buy milk and bread","tags":["home"]}'
show curl -s "$URL/tags"
show curl -s -X POST "$URL/notes" -H "Content-Type: application/json" -d '{"title":""}'
show curl -s -o /dev/null -w "DELETE status %{http_code}" -X DELETE "$URL/notes/2"
