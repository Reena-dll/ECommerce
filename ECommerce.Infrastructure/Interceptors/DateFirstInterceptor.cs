using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace ECommerce.Infrastructure.Interceptors;

public class DateFirstInterceptor : DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {

        SetDateFirst(connection);
        base.ConnectionOpened(connection, eventData);
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await SetDateFirstAsync(connection, cancellationToken);
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    private void SetDateFirst(DbConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = "SET DATEFIRST 1";
        command.ExecuteNonQuery();
    }

    private async Task SetDateFirstAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        var command = connection.CreateCommand();
        command.CommandText = "SET DATEFIRST 1";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
