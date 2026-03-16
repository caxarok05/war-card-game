# War Card Game

This project is an implementation of the classic **War** card game made with **Unity** and **C#**. The game follows a simple rule set where two players draw cards from their decks, and the higher card wins the round. In this version, the player competes against a computer opponent.

The project simulates a **client–server architecture**. Game logic is handled on a fake server (`FakeWarServer`), while the client is responsible for UI, animations, and sending requests to the server.

If you want to start exploring the client code, entry point is **`GameFlowLogic`**, this is main orchestrator of client logic.

The server side contains the core game state and rules implementation, separated from the client to mimic a real multiplayer setup.
