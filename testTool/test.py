# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import csv
import sys
import pandas as pd
import pyodbc as pyodbc
import random

def getCashQuery():
    cnxn_str = ("Driver={SQL Server};"
                "Server=localhost;"
                "Database=BankDataBase;"
                "UID=sa;"
                "PWD=STRONGpassword123!;")
    cnxn = pyodbc.connect(cnxn_str)
    query = "SELECT * FROM Account"
    df_before = pd.read_sql_query(query, cnxn)
    return df_before['CashAmount'].tolist()

def print_hi(name):
    # Use a breakpoint in the code line below to debug your script.
    print(f'Hi, {name}')  # Press Ctrl+F8 to toggle the breakpoint.


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    request_number = input("input number of rows in test csv\n")
    database_state_before = open('database_state_before.txt', 'w+')
    cashAmountBefore = getCashQuery()
    #for account_value in cashAmountBefore:
        #database_state_before.write(str(account_value)+"\n")

    with open('request.csv', 'w', newline='') as file:
        writer = csv.writer(file)
        writer.writerow(["from", "to", "amount"])
        for iteration in range(int(request_number)):
            # zapytanie
            fromUser =random.randint(0, len(cashAmountBefore)-1)
            toUser = random.randint(0, len(cashAmountBefore)-1)
            amount = random.randint(1, 1000)
            writer.writerow([str(fromUser),str(toUser),str(amount)])
            if(cashAmountBefore[fromUser]-amount>=0):
                cashAmountBefore[fromUser]-=amount
                cashAmountBefore[toUser]+=amount

    print("press enter when tests end :-)")
    input()
    print("checking database consistency")
    cashAmountAfter = getCashQuery()
    for user in range(len(cashAmountBefore)):
        if cashAmountBefore[user]!=cashAmountAfter[user]:
            print("DATABASE ERROR")
            print("---> User nr;"+str(user)+" should be:"+str(cashAmountBefore[user]) +" but is:"+str(cashAmountAfter[user]))

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
