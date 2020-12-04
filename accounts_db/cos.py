import random

for i in range(10000):
    print(f"({i},{random.randint(0,1000000)})", end=",")
    if i%995 == 0 and i > 0:
        print(";\nINSERT INTO BankDataBase.dbo.Account VALUES ", end="")