import random
import asyncio
import sys

from aiohttp import ClientSession
import pyodbc
import pandas as pd
import time

async def fetch(url, session):
    async with session.get(url) as response:
        #print(response.status)
        if response.status != 200:
            raise NameError("RESPONSE STATUS: "+response.status)
        return await response.read()

async def bound_fetch(sem, url, session):
    # Getter function with semaphore.
    async with sem:
        await fetch(url, session)

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

async def run(r, t):
    tasks = []
    # create instance of Semaphore
    sem = asyncio.Semaphore(t)

    # Create client session that will ensure we dont open new connection
    # per each request.
    async with ClientSession() as session:
        for i in range(r):
            from_id.append(random.randint(0, len(cashAmountBefore) - 1))
            to_id.append(random.randint(0, len(cashAmountBefore) - 1))
            amount.append(random.randint(1, 1000))
            url = f'http://localhost:8000/send/przelew/{from_id[-1]}/{to_id[-1]}/{amount[-1]}'
            # pass Semaphore and session to every GET request
            task = asyncio.ensure_future(bound_fetch(sem, url.format(i), session))
            tasks.append(task)

        responses = asyncio.gather(*tasks)
        await responses


number_of_requests = int(sys.argv[1])
number_of_concurrent_tasks = 100

print("************Starting program************\n")
print("Creating "+str(number_of_requests)+" requests, where number of concurrent tasks is " + str(number_of_concurrent_tasks))

from_id = []
to_id = []
amount = []
cashAmountBefore = getCashQuery()

loop = asyncio.get_event_loop()

future = asyncio.ensure_future(run(number_of_requests, number_of_concurrent_tasks))

start_time = time.time()
loop.run_until_complete(future)
duration = time.time() - start_time

one_request_time = duration/number_of_requests

print("All requests sent in: "+str(duration)+"s")
print("Average time of one request: "+str(one_request_time)+"s")
print("Number of requests in 1 second: "+str(number_of_requests/duration))

for index in range(len(from_id)):
    if cashAmountBefore[from_id[index]] >= amount[index]:
        cashAmountBefore[from_id[index]] -= amount[index]
        cashAmountBefore[to_id[index]] += amount[index]

print("Press enter when tasks end")
input()

cashAmountAfter = getCashQuery()

for index in range(len(cashAmountBefore)):
    if cashAmountBefore[index] != cashAmountAfter[index]:
        print("Error at id="+str(index))
        print("Amount in database is:" + str(cashAmountAfter[index]))
        print("Should be:" + str(cashAmountBefore[index]))

print("TESTS PASSED")
print("***********Ending program************\n")