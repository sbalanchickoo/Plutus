# Plutus

## Introduction

This solution is named Plutus, after the Greek god of wealth.
This solution and repository contains projects required for Business Account tracking.

The Projects accomplish the following

### Inputs

- Bank transactions  
  Take in OFX files (obtained from Bank) and output Json and Xml via API call
- Bank metadata  
  Take in Bank metadata via csv files (obtained from accounting portal) and output Json and Xml via API call
- Expenses  
  Take in Expense data via csv files (obtained from accounting portal) and output Json and Xml via API call
- Invoices  
  Take in Invoice data via csv files (obtained from accounting portal) and output Json and Xml via API call

### Processing

- Merchant management
  - Assign a new Merchant as one of existing ones
  - If none of the existing ones apply, then create a new one

### General functionality

- Data inputs
  - Local file system
  - Azure Data lake
- Deployment
  - Can be via Docker containers
  - Also possible to run locally using Kestrel
- Logging 
  - Throughout the application using NLog
