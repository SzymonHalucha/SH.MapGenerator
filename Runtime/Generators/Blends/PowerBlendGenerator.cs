namespace SH.MapGenerator.Generators.Blends
{
    public class PowerBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("PowerBlend");
    }
}