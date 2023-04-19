# FastAPI Practice program

from fastapi import FastAPI # Importing FastAPI Library

app = FastAPI() # app is a FastAPI object

@app.get("/")   # Call decorator on app and HTML request "get"
def root():     # What does this function do?
    return { str : "The server is currently up." } # Return string 