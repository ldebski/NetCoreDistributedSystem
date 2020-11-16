# FIRST ARGUMENT - NUMBER OF REQUEST IN SECOND
# SECOND ARGUMENT - NUMBER OF SECONDS

import sys
import asyncio
import pyodbc
import pandas as pd
import random
import requests
import time
from aiohttp import ClientSession

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

def requestSync(from_id, to_id, amount):
    x = requests.get(f'http://localhost:8000/send/przelew/{from_id}/{to_id}/{amount}')
    if x.status_code != 200:
        raise NameError("RETURNED: "+str(x.status_code))

async def runManyRequestsSync():
    #for iteration in range(requests_in_sec * seconds_num):
    for iteration in range(requests_in_sec):
        # zapytanie
        from_id.append(random.randint(0, len(cashAmountBefore)))
        to_id.append(random.randint(0, len(cashAmountBefore)))
        amount.append(random.randint(1, 1000))

        requestSync(from_id[-1], to_id[-1], amount[-1])
        #time.sleep(wait_time)

async def requestAsync(url):
    async with ClientSession() as session:
        async with session.get(url) as response:
            response = await  response.text()

from_id=[]
to_id =[]
amount =[]
requests_in_sec=int(sys.argv[1])
#wait_time=1/int(sys.argv[1])
#seconds_num=int(sys.argv[2])

#sciagniecie stanu bazy
cashAmountBefore = getCashQuery()

# make requests
loop = asyncio.get_event_loop()
tasks = []
przed=time.time()
for i in range(requests_in_sec):
    from_id.append(random.randint(0, len(cashAmountBefore)-1))
    to_id.append(random.randint(0, len(cashAmountBefore)-1))
    amount.append(random.randint(1, 1000))
    url=f'http://localhost:8000/send/przelew/{from_id[-1]}/{to_id[-1]}/{amount[-1]}'
    task = asyncio.ensure_future(requestAsync(url))
    tasks.append(task)

loop.run_until_complete(asyncio.wait(tasks))

print(time.time()-przed)
#wyliczenie prawidlowego stanu bazy
for index in range(len(from_id)):
    if cashAmountBefore[from_id[index]] >= amount[index]:
        cashAmountBefore[from_id[index]] -= amount[index]
        cashAmountBefore[to_id[index]] += amount[index]

#sciagniecie stanu bazy
cashAmountAfter = getCashQuery()

#porownanie stanu bazy
for index in range(len(cashAmountBefore)):
    if cashAmountBefore[index] != cashAmountAfter[index]:
        print(cashAmountBefore[index])
        print(cashAmountAfter[index])
        print(index)
        raise NameError("BLEDNY STAN BAZY")

print("TESTS PASSED")