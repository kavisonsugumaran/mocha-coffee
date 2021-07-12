# Project Variables
PROJECT_NAME ?= MochaCoffee
ORG_NAME ?= MochaCoffee
REPO_NAME ?= MochaCoffee

.PHONY: migrations db

migrations:
	cd ./MochaCoffee.Data && dotnet ef --startup-project ../MochaCoffee.Web/ migrations add $(mname) && cd..

db:
	cd ./MochaCoffee.Data && dotnet ef --startup-project ../MochaCoffee.Web/ database update && cd..
