# 🎯 EventSourcing MemberShip Project

A clean implementation of **Event Sourcing** and **CQRS** (Command Query Responsibility Segregation) patterns applied to a Membership domain, built with **.NET / C#**.

---

## 📐 Architecture Overview

This project follows the **CQRS + Event Sourcing** architecture, separating the write side (Commands) from the read side (Queries) into two distinct services.

```
┌─────────────────────────────────────────────────────┐
│                   Client / API                      │
└────────────────────┬──────────────────┬─────────────┘
                     │                  │
           ┌─────────▼──────┐  ┌───────▼──────────┐
           │ MemberShip     │  │  MemberShip       │
           │  .Command      │  │   .Query          │
           │  (Write Side)  │  │  (Read Side)      │
           └─────────┬──────┘  └───────┬──────────┘
                     │                  │
           ┌─────────▼──────┐  ┌───────▼──────────┐
           │  Event Store   │  │   Read Model /    │
           │  (Source of    │─►│   Projections     │
           │   Truth)       │  │                   │
           └────────────────┘  └──────────────────┘
```

> See `CurrentState.png` in the repository root for the full system diagram.

---

## 🗂️ Project Structure

```
EventSourcing-MemberShip-Project/
│
├── MemberShip.Command/       # Write side — handles Commands & stores Events
│   ├── Commands/             # Command definitions
│   ├── Handlers/             # Command handlers (business logic)
│   ├── Aggregates/           # Domain aggregates (event application)
│   └── Events/               # Domain events
│
├── MemberShip.Query/         # Read side — handles Queries & serves Read Models
│   ├── Queries/              # Query definitions
│   ├── Handlers/             # Query handlers
│   └── Projections/          # Event projections → read model updates
│
└── CurrentState.png          # Architecture / state diagram
```

---

## 🚀 Key Concepts

### Event Sourcing
Instead of storing the current state, every state change is captured as an immutable **Domain Event** and appended to an event store. The current state can be reconstructed at any point by replaying the event stream.

### CQRS
| Aspect | Command Side | Query Side |
|---|---|---|
| **Purpose** | Mutate state | Read state |
| **Input** | Commands | Queries |
| **Output** | Domain Events | Read Models / DTOs |
| **Storage** | Event Store | Optimized Read DB |

---

## 🔧 Configuration

This project uses **Azure Service Bus (Queue)** to publish domain events from the Command side to the Query side via the **Outbox Pattern**.

### Required Credentials

The project reads secrets via **[.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)**. Set them up in the `MemberShip.Command` project:

```bash
dotnet user-secrets set "ServiceBus:ConnectionString" "<your-azure-service-bus-connection-string>"
```

| Key | Description |
|---|---|
| `ServiceBus:ConnectionString` | Azure Service Bus namespace connection string |

or set them in your appsettings.json...etc.

> **Queue name** is `anis-membership-ayham` — hardcoded in `ServiceBusPublisher`.

### How to get the Connection String

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to your **Service Bus Namespace**
3. Go to **Settings → Shared access policies**
4. Copy the **Primary Connection String**

---

## ⚙️ Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later recommended)
- Visual Studio 2022 / VS Code / Rider

### Clone the repository

```bash
git clone https://github.com/AyhamAlaama/EventSourcing-MemberShip-Project.git
cd EventSourcing-MemberShip-Project
```

### Build the solution

```bash
dotnet build
```

### Run the Command service

```bash
cd MemberShip.Command
dotnet run
```

### Run the Query service

```bash
cd MemberShip.Query
dotnet run
```

---

## 🧩 How It Works

1. **Client sends a Command** (e.g., `RegisterMemberCommand`) to the Command service.
2. The **Command Handler** validates and processes the command via the **Aggregate**.
3. The Aggregate raises a **Domain Event** (e.g., `MemberRegisteredEvent`) and persists it to the **Event Store**.
4. The event is saved to the **Outbox table** in the same transaction (Outbox Pattern).
5. `ServiceBusPublisher` picks up outbox messages and sends them to the Azure Service Bus **Queue** (`anis-membership-ayham`).
6. The **Query service** consumes messages from the queue and updates its **Read Model**.
7. **Client queries** the Query service to retrieve the current state from the optimized read model.

---

## 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| **C# / .NET** | Core language & framework |
| **Event Sourcing** | State persistence pattern |
| **CQRS** | Architectural separation of reads and writes |
| **Domain-Driven Design (DDD)** | Aggregates, domain events, bounded context |

---

## 📸 Current State Diagram

![Current State](./CurrentState.png)

## 📄 License

This project is open source. Feel free to use it as a reference or learning resource.

---

> Built by [@AyhamAlaama](https://github.com/AyhamAlaama)
