namespace PipelineDreams
{
    public class InstructionFluctuation : Instruction {
        public InstructionFluctuation(EntityDataContainer eM, Entity player, CommandsContainer pC, InstructionData data, string variant) : base(eM, player, pC, data, variant) {
        }

        public override IClockTask Operation(float startClock)
        {

            return new InstructionFluctuationTask();
        }
        
    }
    public abstract partial class Instruction
    {
        /// <summary>
        /// Field instruction task used above.
        /// </summary>
        protected class InstructionFluctuationTask : InstructionTask
        {
            protected override void OnRunStart()
            {
            }
        }
    }

}
