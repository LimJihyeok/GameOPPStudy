using System;
using System.Collections.Generic;
using System.Text;

namespace GameOPPStudy
{
    internal class GameManager
    {
        private bool _isRunning = true;
        private Renderer _renderer; 
        private Player _player;
        private List<Entity> _entities;

        // 12 프레임 설정 (1초 / 12 = 약 83밀리초)
        private const int TargetFps = 12;
        private const int FrameIntervalMs = 1000 / TargetFps;

        public GameManager()
        {
            _renderer = new Renderer(80, 25); // 80열 25행 콘솔 크기 기준
            _entities = new List<Entity>();

            // 플레이어 생성 및 리스트 추가
            _player = new Player(10, 5, _renderer);
            _entities.Add(_player);

            // 다른 엔티티(NPC나 아이템 등) 여러 개 추가 가능
            _entities.Add(new Npc(30, 10, "X", _renderer));
            _entities.Add(new Npc(50, 15, "#", _renderer));
        }

        public void Run()
        {
            Console.CursorVisible = false;
            long previousTick = DateTime.Now.Ticks;

            while (_isRunning)
            {
                long currentTick = DateTime.Now.Ticks;
                long elapsedMs = (currentTick - previousTick) / TimeSpan.TicksPerMillisecond;

                if (elapsedMs >= FrameIntervalMs)
                {
                    previousTick = currentTick;

                    ProcessInput();
                    Update();
                    Render();
                }
                else
                {
                    int sleepTime = (int)(FrameIntervalMs - elapsedMs);
                    if (sleepTime > 0)
                    {
                        Thread.Sleep(sleepTime);
                    }
                }
            }

            Console.CursorVisible = true;
        }

        private void ProcessInput()
        {
            ConsoleKeyInfo? frameKey = null;

            // 1. 이번 프레임의 유효한 입력 딱 하나만 가져오기
            if (Console.KeyAvailable)
            {
                frameKey = Console.ReadKey(true);

                if (frameKey.Value.Key == ConsoleKey.Escape)
                {
                    _isRunning = false;
                }
            }

            // 2. 남은 버퍼 비우기 (점프/입력 밀림 방지)
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            // 플레이어 엔티티에게 입력값 전달
            _player.SetInput(frameKey);
        }

        private void Update()
        {
            // 관리하는 모든 엔티티들의 Update 실행
            foreach (var entity in _entities)
            {
                entity.Update();
            }
        }

        private void Render()
        {
            _renderer.Clear();

            // UI 및 배경 그리기
            _renderer.DrawBox(0, 0, _renderer.Width, _renderer.Height);
            _renderer.DrawString(2, 1, "=== C# 12 FPS Entity Text Game ===");
            _renderer.DrawString(2, 2, "Move: W, A, S, D / Quit: ESC");

            // 모든 엔티티들의 Render 실행 (위치와 값들을 받아 화면 버퍼에 그림)
            foreach (var entity in _entities)
            {
                entity.Render();
            }

            // 최종 화면 출력
            _renderer.Render();
        }
    }

    // 화면 출력을 담당하는 렌더러 클래스
    public class Renderer
    {
        public int Width { get; }
        public int Height { get; }
        private StringBuilder _screenBuffer;

        public Renderer(int width, int height)
        {
            Width = width;
            Height = height;
            _screenBuffer = new StringBuilder(width * height);

            try
            {
                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);
            }
            catch { }
        }

        public void Clear()
        {
            _screenBuffer.Clear();
            for (int y = 0; y < Height; y++)
            {
                _screenBuffer.AppendLine(new string(' ', Width - 1));
            }
        }

        public void DrawString(int x, int y, string str)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;

            int index = y * (Width + 1) + x;
            if (index + str.Length <= _screenBuffer.Length)
            {
                _screenBuffer.Remove(index, str.Length);
                _screenBuffer.Insert(index, str);
            }
        }

        public void DrawBox(int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                DrawString(x, startY, "-");
                DrawString(x, startY + height - 1, "-");
            }
            for (int y = startY; y < startY + height; y++)
            {
                DrawString(startX, y, "|");
                DrawString(startX + width - 1, y, "|");
            }
        }

        public void Render()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(_screenBuffer.ToString());
        }
    }
}
