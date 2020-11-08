import requests
import random
import pyodbc
import pandas as pd

NUMBER_OF_QUERIES = 100000

def main():
    cashAmountBefore = getCashQuery()
    fromID, toID, amount = runHttpRequests(len(cashAmountBefore))
    referenceCashAmount = calculateReferenceCashAmount(cashAmountBefore, fromID, toID, amount)
    input('Press enter if all queries finished.\n')
    cashAmountAfter = getCashQuery()
    compare(cashAmountAfter, referenceCashAmount)

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


def runHttpRequests(cashAmountBefore):
    fromID, toID, amount = [], [], []

    for i in range(NUMBER_OF_QUERIES):
        if i % 1000 == 0:
            print(i)
        fromID.append(random.randint(0, cashAmountBefore))
        toID.append(random.randint(0, cashAmountBefore))
        amount.append(random.randint(100, 99999))
        x = requests.get(f'http://localhost:8000/send/przelew/{fromID[-1]}/{toID[-1]}/{amount[-1]}')
        if x.status_code != 200:
            print("oh shieeeet")
            break

    return fromID, toID, amount


def calculateReferenceCashAmount(cashAmountBefore, fromID, toID, amount):
    referenceCashAmount = cashAmountBefore.copy()
    for i in range(len(fromID)):
        if referenceCashAmount[fromID[i]] >= amount[i]:
            referenceCashAmount[fromID[i]] -= amount[i]
            referenceCashAmount[toID[i]] += amount[i]
    return referenceCashAmount


def compare(cashAmountAfter, referenceCashAmount):
    oh_shiet = False
    for cashDB, cashReference in zip(cashAmountAfter, referenceCashAmount):
        if cashDB != cashReference:
            print(f"{cashDB} should be {cashReference}")
            oh_shiet = True
    if not oh_shiet:
        print("Noice")

if __name__ == "__main__":
    main()
