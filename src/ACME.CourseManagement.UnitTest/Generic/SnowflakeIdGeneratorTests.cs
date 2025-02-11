using ACME.CourseManagement.Service.Domain.Helpers;

namespace ACME.CourseManagement.UnitTest.Generic;

public class SnowflakeIdGeneratorTests
{
    private readonly SnowflakeIdGenerator _generator;

    public SnowflakeIdGeneratorTests()
    {
        _generator = new SnowflakeIdGenerator(workerId: 1, datacenterId: 1);
    }

    /// <summary>
    /// Verifica que el constructor lance una excepción cuando el WorkerId es inválido.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_Exception_When_WorkerId_Is_Invalid()
    {
        Action act = () => new SnowflakeIdGenerator(-1, 1);
        act.Should().Throw<ArgumentException>().WithMessage("Worker ID debe estar entre 0 y *");
    }

    /// <summary>
    /// Verifica que el constructor lance una excepción cuando el DatacenterId es inválido.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_Exception_When_DatacenterId_Is_Invalid()
    {
        Action act = () => new SnowflakeIdGenerator(1, -1);
        act.Should().Throw<ArgumentException>().WithMessage("Datacenter ID debe estar entre 0 y *");
    }

    /// <summary>
    /// Verifica que los IDs generados sean únicos.
    /// </summary>
    [Fact]
    public void NextId_Should_Generate_Unique_Ids()
    {
        var ids = new HashSet<long>();
        for (int i = 0; i < 1000; i++)
        {
            long id = _generator.NextId();
            ids.Add(id);
        }
        ids.Count.Should().Be(1000, because: "cada ID generado debe ser único");
    }

    /// <summary>
    /// Verifica que se lance una excepción si el reloj del sistema retrocede.
    /// </summary>
    [Fact]
    public void NextId_Should_Throw_Exception_If_Clock_Moves_Backwards()
    {
        var generator = new SnowflakeIdGenerator(1, 1);

        generator.NextId();

        // Simula un caso donde el reloj retrocede
        typeof(SnowflakeIdGenerator)
            .GetField("_lastTimestamp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(generator, long.MaxValue);

        Action act = () => generator.NextId();
        act.Should().Throw<InvalidOperationException>().WithMessage("El reloj se movió hacia atrás. No se pueden generar IDs");
    }

    /// <summary>
    /// Verifica que la generación de IDs funcione correctamente en concurrencia.
    /// </summary>
    [Fact]
    public void NextId_Should_Work_Concurrently()
    {
        var generator = new SnowflakeIdGenerator(1, 1);
        var tasks = new List<Task<long>>();
        var ids = new HashSet<long>();
        var lockObject = new object();

        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                long id = generator.NextId();
                lock (lockObject)
                {
                    ids.Add(id);
                }
                return id;
            }));
        }

        Task.WhenAll(tasks).Wait();
        ids.Count.Should().Be(100, because: "cada ID generado debe ser único incluso en concurrencia");
    }
}