#!/usr/bin/env bash

# Wait for database to startup 
sleep 20
echo "[INFO] Running createDB.sql script"
./opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P STRONGpassword123! -i createDB.sql