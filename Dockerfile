# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln ./
COPY src/ ./src/
WORKDIR /app/src/PuduSimulator
RUN dotnet restore
RUN dotnet publish -c Release -o /publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

ENV DB_PATH=/data/pudu.db
VOLUME /data

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "PuduSimulator.dll"]
