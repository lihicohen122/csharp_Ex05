using System;
using Ex05_Logic.Enums;

namespace Ex05_Logic
{
    internal class MonteCarloAI
    {
        private const int k_NotFound = -1;
        private const int k_WinWeight = 10;
        private const int k_TieWeight = 2;
        private const int k_LoseWeight = -50; 
        private const int k_SimulationsPerMove = 500;
        private readonly Random r_Random;

        private int simulateSinglePlayRound(Board i_SimulatedBoard, int i_FirstMoveRow, int i_FirstMoveColumn,
                                            eCellSign i_ComputerSign, eCellSign i_HumanSign)
        {
            int playOutScore;

            i_SimulatedBoard.TryUpdateCell(i_FirstMoveRow, i_FirstMoveColumn, i_ComputerSign);
            bool isGameOver = i_SimulatedBoard.CheckWinningSequence(i_FirstMoveRow, i_FirstMoveColumn, i_ComputerSign);

            if(isGameOver)
            {
                playOutScore = k_LoseWeight;
            }
            else
            {
                playOutScore = completeSimulationUntilEnd(i_SimulatedBoard, i_ComputerSign, i_HumanSign);
            }

            return playOutScore;
        }

        private int completeSimulationUntilEnd(Board i_SimulatedBoard, eCellSign i_ComputerSign, eCellSign i_HumanSign)
        {
            int playOutScore = k_TieWeight;
            eCellSign currentTurnSign = i_HumanSign;
            bool isGameOver = false;

            while(!isGameOver && !i_SimulatedBoard.IsBoardFull())
            {
                executeRandomMove(i_SimulatedBoard, currentTurnSign, out isGameOver);
                if(isGameOver)
                {
                    playOutScore = currentTurnSign == i_HumanSign ? k_WinWeight : k_LoseWeight;
                }
                else
                {
                    currentTurnSign = currentTurnSign == i_HumanSign ? i_ComputerSign : i_HumanSign;
                }
            }

            return playOutScore;
        }

        private void executeRandomMove(Board i_Board, eCellSign i_CurrentTurnSign, out bool o_IsGameOver)
        {
            getRandomEmptyCell(i_Board, out int randomRow, out int randomColumn);
            i_Board.TryUpdateCell(randomRow, randomColumn, i_CurrentTurnSign);
            o_IsGameOver = i_Board.CheckWinningSequence(randomRow, randomColumn, i_CurrentTurnSign);
        }

        private void getRandomEmptyCell(Board i_Board, out int o_Row, out int o_Column)
        {
            int emptyCellsCount = i_Board.NumberOfEmptyCells;
            int randomEmptyIndex = r_Random.Next(0, emptyCellsCount);
            int currentEmptyCount = 0;
            int boardSize = i_Board.BoardSize;
            bool isFound = false;

            o_Row = k_NotFound;
            o_Column = k_NotFound;
            for(int row = 0; row < boardSize && !isFound; ++row)
            {
                for(int column = 0; column < boardSize && !isFound; ++column)
                {
                    if(i_Board.GetCell(row, column) == eCellSign.Empty)
                    {
                        if(currentEmptyCount == randomEmptyIndex)
                        {
                            o_Row = row;
                            o_Column = column;
                            isFound = true;
                        }

                        currentEmptyCount++;
                    }
                }
            }
        }

        public MonteCarloAI()
        {
            r_Random = new Random();
        }
        
        public void StartMonteCarloTreeSearchAlgorithm(Board i_CurrentBoard, eCellSign i_ComputerSign, eCellSign i_HumanSign,
                                                       out int o_BestRow, out int o_BestColumn)
        {
            int bestScore = int.MinValue;
            int boardSize = i_CurrentBoard.BoardSize;
            Board simulatedBoard = i_CurrentBoard.CloneBoard();

            o_BestRow = k_NotFound;
            o_BestColumn = k_NotFound;
            for(int row = 0; row < boardSize; ++row)
            {
                for(int column = 0; column < boardSize; ++column)
                {
                    if(i_CurrentBoard.GetCell(row, column) == eCellSign.Empty)
                    {
                        int currentMoveScore = 0;

                        for(int simulation = 0; simulation < k_SimulationsPerMove; ++simulation)
                        {
                            simulatedBoard.RestoreState(i_CurrentBoard);
                            currentMoveScore += simulateSinglePlayRound(simulatedBoard, row, column, i_ComputerSign, i_HumanSign);
                        }

                        if(currentMoveScore > bestScore)
                        {
                            bestScore = currentMoveScore;
                            o_BestRow = row;
                            o_BestColumn = column;
                        }
                    }
                }
            }
        }
    }
}