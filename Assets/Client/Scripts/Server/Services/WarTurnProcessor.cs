namespace Client.Scripts.Shared
{
    public sealed class WarTurnProcessor
    {
        private readonly WarResolutionService _warResolutionService;

        public WarTurnProcessor(WarResolutionService warResolutionService)
        {
            _warResolutionService = warResolutionService;
        }

        public DrawResponse Process(WarGameState state)
        {
            return _warResolutionService.ResolveTurn(state);
        }
    }
}