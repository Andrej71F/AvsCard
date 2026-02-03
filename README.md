Here’s your full GitHub-style `README.md` for the entire AVS Card Integration Suite repository, formatted cleanly and consistently:

```md
# AVS Card Integration Suite

This repository contains a complete set of tools for developing, testing, and validating AVS Card transaction flows.  
It includes:

- **AVS Client Library** – a reusable .NET client for communicating with the AVS Transaction Gateway  
- **WPF Test Application** – a UI tool for manual testing and debugging  
- **Unit Tests** – automated tests validating request/response handling and core logic  

The suite is designed for developers integrating AVS Card functionality into payment systems, simulators, or backend services.

---

## Repository Structure

```
/AvsCard.Client
    Core AVS client library
    Request/response DTOs
    XML serialization
    HTTP transport
    Logging integration

/AvsCard.Wpf.Test
    WPF test application
    MVVM (Prism)
    Async execution
    UI overlays and modal wait dialogs
    RichTextBox output log
    Config-driven credentials

/AvsCard.Tests
    Unit tests for client logic
    Request factory tests
    Response parsing tests
    Error handling tests
```

---

## AVS Client Library

The `AvsCard.Client` project provides:

- Strongly typed request/response models  
- Request factory (`AvsRequestFactory`)  
- XML serialization and deserialization  
- HTTP transport using `HttpClient`  
- Optional `ILogger` injection  
- Synchronous API (wrapped in async by consumers)  

### Supported Operations

- `BALANCE`
- `REDEEM`
- `REFUND`
- `CANCEL`

Each operation produces a typed response (e.g., `AvsBalanceResponse`).

---

## WPF Test Application

The `AvsCard.Wpf.Test` project is a standalone UI tool for manual testing.

### Features

- Load configuration from `appsettings.test.json`
- Execute AVS operations with one click
- Modal “Please wait…” dialog
- Optional semi-transparent overlay
- RichTextBox output log
- Structured logging via `Microsoft.Extensions.Logging`
- Background execution (UI never freezes)
- Automatic TxId generation

### Configuration File

The application reads settings from:

```
appsettings.test.json
```

Example:

```json
{
  "Avs": {
    "Endpoint": "https://up.test.epayworldwide.com/tgxml",
    "Username": "test_task999999",
    "Password": "Dummy",
    "TerminalId": "test_task999999",
    "TestPan": ";6364530000029556=49121011234?"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

#### AVS Settings

| Key         | Description                         |
|-------------|-------------------------------------|
| Endpoint    | AVS gateway URL                     |
| Username    | AVS account username                |
| Password    | AVS account password                |
| TerminalId  | Terminal identifier                 |
| TestPan     | Test PAN (track‑2 format supported) |

#### Logging Settings

Controls verbosity of logs:

- `Default`: global log level  
- `Microsoft`: overrides framework logs  

Logs include:

- Configuration loading  
- Request creation  
- Request execution  
- Response details  
- Validation errors  
- Exceptions  

---

## Unit Tests

The `AvsCard.Tests` project includes:

- Request factory tests  
- Response parsing tests  
- Error handling tests  
- Serialization/deserialization tests  

Tests ensure:

- Correct XML structure  
- Correct request generation  
- Robust error handling  
- Accurate response mapping  

Run tests using your preferred test runner (e.g., Visual Studio Test Explorer or `dotnet test`).

---

## Getting Started

1. Clone the repository  
2. Restore NuGet packages  
3. Build the solution  
4. Ensure `appsettings.test.json` is present in `/AvsCard.Wpf.Test/bin/Debug/net6.0-windows`  
5. Run the WPF test application  
6. Use the UI to send AVS requests and inspect responses  

---

## Notes

- PasswordBox does not support binding; password is applied manually in code-behind  
- All AVS requests run on a background thread to keep UI responsive  
- The client is intended for testing and development only  

---

## License

This project is licensed under the MIT License. See the `[Anscheinend war das Ergebnis nicht sicher anzuzeigen. Lassen Sie uns die Dinge ändern und etwas anderes ausprobieren!]` file for details.
```

Let me know if you want a version with badges, contributor credits, or CI/CD instructions. I can also generate a changelog or release notes if you're preparing for a public release.
