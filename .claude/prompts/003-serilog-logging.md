# Epic 2 - Task 2.3
## Serilog Logging

Read before making changes:

- CLAUDE.md
- Existing solution
- Existing configuration

Objective:

Configure enterprise logging using Serilog.

Requirements

1. Configure Serilog using appsettings.json.

2. Create:

Api/Extensions/LoggingExtensions.cs

The extension method should:

- Configure Serilog from configuration
- Read settings from the Serilog section in appsettings.json
- Enrich logs with:
  - Machine Name
  - Environment Name
  - Thread Id
  - Process Id
- Enable console logging
- Enable rolling daily file logging under:
  logs/log-.txt

3. Keep Program.cs minimal.

Program.cs should only call something similar to:

builder.Host.AddApplicationLogging(builder.Configuration);

4. Create:

Shared/Constants/LoggingConstants.cs

Include constants such as:

- CorrelationId
- UserId
- UserName

5. Add the required Serilog packages if missing.

6. Add a Serilog section to:

- appsettings.json
- appsettings.Development.json

7. Build the solution.

Constraints

Do NOT:

- Add middleware
- Add controllers
- Add authentication
- Add entities
- Add business logic

Deliverables

- Files modified
- Packages installed
- Build output
- Suggested Git commit