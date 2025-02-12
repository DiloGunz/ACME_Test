namespace ACME.CourseManagement.Service.Domain.Helpers;

/// <summary>
/// Generador de identificadores únicos basado en el algoritmo Snowflake de Twitter.
/// </summary>
public class SnowflakeIdGenerator
{
    /// <summary>
    /// Marca de tiempo base (Twitter original).
    /// </summary>
    private const long Twepoch = 1288834974657L;

    /// <summary>
    /// Número de bits reservados para el identificador del nodo de trabajo.
    /// </summary>
    private const int WorkerIdBits = 5;

    /// <summary>
    /// Número de bits reservados para el identificador del datacenter.
    /// </summary>
    private const int DatacenterIdBits = 5;

    /// <summary>
    /// Número de bits reservados para la secuencia en un mismo milisegundo.
    /// </summary>
    private const int SequenceBits = 12;

    /// <summary>
    /// Valor máximo permitido para el identificador del nodo de trabajo.
    /// </summary>
    private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);

    /// <summary>
    /// Valor máximo permitido para el identificador del datacenter.
    /// </summary>
    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

    /// <summary>
    /// Máscara de secuencia para garantizar la unicidad dentro del mismo milisegundo.
    /// </summary>
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);

    /// <summary>
    /// Número de bits a desplazar para el identificador del nodo de trabajo.
    /// </summary>
    private const int WorkerIdShift = SequenceBits;

    /// <summary>
    /// Número de bits a desplazar para el identificador del datacenter.
    /// </summary>
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;

    /// <summary>
    /// Número de bits a desplazar para la marca de tiempo.
    /// </summary>
    private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

    private readonly long _workerId;
    private readonly long _datacenterId;
    private long _lastTimestamp = -1L;
    private long _sequence = 0L;
    private readonly object _lock = new();

    /// <summary>
    /// Inicializa una nueva instancia del generador de identificadores Snowflake.
    /// </summary>
    /// <param name="workerId">Identificador único del nodo de trabajo.</param>
    /// <param name="datacenterId">Identificador único del datacenter.</param>
    /// <exception cref="ArgumentException">Se lanza si los valores de <paramref name="workerId"/> o <paramref name="datacenterId"/> están fuera de rango.</exception>
    public SnowflakeIdGenerator(long workerId, long datacenterId)
    {
        if (workerId > MaxWorkerId || workerId < 0)
            throw new ArgumentException($"Worker ID debe estar entre 0 y {MaxWorkerId}");

        if (datacenterId > MaxDatacenterId || datacenterId < 0)
            throw new ArgumentException($"Datacenter ID debe estar entre 0 y {MaxDatacenterId}");

        _workerId = workerId;
        _datacenterId = datacenterId;
    }

    /// <summary>
    /// Genera un nuevo identificador único de manera segura y secuencial.
    /// </summary>
    /// <returns>Un identificador único de 64 bits.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si el reloj del sistema retrocede.</exception>
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

    /// <summary>
    /// Obtiene la marca de tiempo actual en milisegundos desde la época Unix.
    /// </summary>
    /// <returns>Marca de tiempo actual en milisegundos.</returns>
    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// Espera hasta que se obtenga el siguiente milisegundo en caso de colisión de secuencia.
    /// </summary>
    /// <param name="lastTimestamp">Última marca de tiempo utilizada.</param>
    /// <returns>Nueva marca de tiempo en milisegundos.</returns>
    private long WaitForNextTimestamp(long lastTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    /// <summary>
    /// Genera un identificador único utilizando una instancia estática con valores predeterminados.
    /// </summary>
    /// <returns>Un identificador único de 64 bits.</returns>
    public static long GenerateId()
    {
        var generator = new SnowflakeIdGenerator(1, 1);
        return generator.NextId();
    }
}



