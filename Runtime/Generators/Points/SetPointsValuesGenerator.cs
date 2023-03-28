namespace SH.MapGenerator.Generators.Points
{
    public class SetPointsValuesGenerator : BasePointsValuesGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("SetPointsValues");
    }
}