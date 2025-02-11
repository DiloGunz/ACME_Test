namespace ACME.CourseManagement.Service.Domain.Helpers;

/// <summary>
/// Clase que genera Id (long) unico para aplicaciones distribuidas mediate algoritmo Twitter Snowflake
/// </summary>
public class SnowflakeIdGenerator
{
    private const long Twepoch = 1288834974657L; // Marca de tiempo base (Twitter original)
    private const int WorkerIdBits = 5;  // Bits para el ID de la máquina (máx 31)
    private const int DatacenterIdBits = 5;  // Bits para el ID del datacenter (máx 31)
    private const int SequenceBits = 12; // Bits para la secuencia en un mismo milisegundo (máx 4095)

    private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);

    private const int WorkerIdShift = SequenceBits;
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
    private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

    private readonly long _workerId;
    private readonly long _datacenterId;
    private long _lastTimestamp = -1L;
    private long _sequence = 0L;
    private readonly object _lock = new();

    public SnowflakeIdGenerator(long workerId, long datacenterId)
    {
        if (workerId > MaxWorkerId || workerId < 0)
            throw new ArgumentException($"Worker ID debe estar entre 0 y {MaxWorkerId}");

        if (datacenterId > MaxDatacenterId || datacenterId < 0)
            throw new ArgumentException($"Datacenter ID debe estar entre 0 y {MaxDatacenterId}");

        _workerId = workerId;
        _datacenterId = datacenterId;
    }

    public long NextId()
    {
        lock (_lock)
        {
            long timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
                throw new InvalidOperationException("El reloj se movió hacia atrás. No se pueden generar IDs");

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & SequenceMask;
                if (_sequence == 0)
                {
                    // Esperar hasta el siguiente milisegundo
                    timestamp = WaitForNextTimestamp(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;

            return ((timestamp - Twepoch) << TimestampLeftShift) |
                   (_datacenterId << DatacenterIdShift) |
                   (_workerId << WorkerIdShift) |
                   _sequence;
        }
    }

    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private long WaitForNextTimestamp(long lastTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    public static long GenerateId()
    {
        var generator = new SnowflakeIdGenerator(1, 1);
        return generator.NextId();
    }
}


