# MarketStack

## Overview

**MarketStack** is a C# project focused on integrating multiple supermarket APIs to collect, process, and manage digital receipts and purchase data in a centralized system. It provides a comprehensive platform for aggregating receipt data from supermarket chains and offering detailed insights into purchasing patterns, product pricing, and tax information.

## Project Vision

### What is MarketStack?

MarketStack is designed to be your personal purchase analytics platform. The primary goal is to help users understand their spending habits by collecting and analyzing receipt data from various supermarket chains in a structured, unified system.

### Why Build MarketStack?

Modern consumers buy groceries from multiple supermarkets, but purchase data is typically scattered across different loyalty apps and receipt formats. MarketStack solves this problem by:

- **Centralizing purchase data** from multiple supermarket APIs into a single database
- **Standardizing receipt information** across different grocery chains
- **Providing actionable insights** about spending, pricing trends, and tax burdens
- **Enabling data ownership** by allowing users to self-host the entire system

### Future Capabilities

MarketStack is evolving into a complete self-hostable analytics platform with two key components:

#### 1. **REST API** (In Development)
- Exposes endpoints for receipt data, product information, and purchase analytics
- Supports automated data collection from integrated supermarket APIs
- Provides real-time and historical analytics queries
- Can be self-hosted for complete data privacy and ownership

#### 2. **Web Dashboard** (Planned)
- Interactive visualization of purchase metrics and spending trends
- Real-time analytics and reporting
- Price comparison across different supermarket chains
- Personal spending insights and historical data exploration
- Full self-hosting capability for users who want to maintain complete control over their data

## Key Features

### Current Features

- **Multi-Supermarket Integration**: Fetch receipts from integrated supermarket APIs (currently Lidl)
- **Receipt Data Collection**: Automatic retrieval and storage of receipt information including:
  - Receipt metadata (date, store, chain)
  - Item details (product names, prices, quantities)
  - Price information (pre-tax, taxes, total expenses)
- **Data Analysis Services**:
  - Calculate total purchase expenses
  - Compute total tax paid
  - Track spending patterns
- **REST API**: Access collected data programmatically
- **Database Persistence**: Store and retrieve historical purchase data
- **Background Job Processing**: Automated data collection via scheduled jobs (Hangfire)

### Planned Features

- Additional supermarket API integrations (Dm, Edeka, Rewe, Aldi, etc.)
- Advanced analytics and visualization dashboard
- Budget tracking
- Price trend analysis and comparison
- User authentication and multi-user support
- Self-hosting deployment guide and Docker support

## Architecture

MarketStack follows a layered architecture with clear separation of concerns:

### Project Structure

- **MarketStack.API**: REST API and HTTP endpoints for data access
- **MarketStack.Logic**: Business logic and service layer (price analysis, receipt processing)
- **MarketStack.Data**: Database context, repositories, and data persistence (Entity Framework Core)
- **MarketStack.Library**: Integration with supermarket APIs and external services
- **MarketStack.Common**: Shared utilities, response models, and cross-cutting concerns
- **Contracts**: Interfaces and data transfer objects (DTOs) for decoupled communication

### Technology Stack

- **Framework**: .NET 10
- **Database**: Entity Framework Core with Postgres and migrations
- **API Documentation**: NSwag/OpenAPI (Swagger)
- **Job Scheduling**: Hangfire for background task processing
- **Architecture**: Clean Architecture principles with dependency injection

## Data Model

### Core Entities

- **Receipts**: Purchase transactions from supermarket chains
- **Receipt Items**: Individual products purchased in each receipt
- **Products**: Product information and metadata
- **Price Summaries**: Aggregated price data for tax and expense tracking
- **Tags**: Categorization and organization of purchase data

## Getting Started

> ⚠️ **Note**: MarketStack is still in active development. Setup instructions for self-hosting will be provided once the project reaches a stable release.

## Privacy and Data Control

One of the core principles of MarketStack is **data ownership**. Users will be able to self-host the entire system (API + dashboard), ensuring:

- Complete control over personal purchase data
- No third-party data sharing
- Privacy-compliant data storage and processing
- Ability to delete or export data at any time

## Contributing

This is a personal project, but contributions, suggestions, and feature requests are welcome. Feel free to open issues or discussions on GitHub.

## Contact & Support

For updates and more information, visit the [MarketStack GitHub repository](https://github.com/Malik0502/MarketStack).

---

**Status**: Active Development

**Last Updated**: 22.08.2026

*MarketStack - Take control of your grocery spending data.*
