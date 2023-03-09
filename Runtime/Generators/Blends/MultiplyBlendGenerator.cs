namespace SH.MapGenerator.Generators.Blends
{
    public class MultiplyBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("MultiplyBlend");
    }
}