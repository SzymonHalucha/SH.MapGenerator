namespace SH.MapGenerator.Generators.Blends
{
    public class AddBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("AddBlend");
    }
}