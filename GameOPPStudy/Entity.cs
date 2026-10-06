using System;
using System.Collections.Generic;
using System.Text;

namespace GameOPPStudy
{
    // 1. 기본 엔티티(Entity) 클래스
    public abstract class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Symbol { get; set; }

        public Inventory Inventory { get; protected set; }

        protected Renderer RendererRef { get; }

        protected Entity(int x, int y, string symbol, Renderer renderer)
        {
            X = x;
            Y = y;
            Symbol = symbol;
            RendererRef = renderer;
        }

        // 엔티티별 행동(Update) 정의 (다음 행동값 처리 등)
        public abstract void Update();

        // 렌더러에게 위치와 값들을 전달하여 그리기
        public virtual void Render()
        {
            RendererRef.DrawString(X, Y, Symbol);
        }
    }

    // 2. 플레이어 엔티티 (키 입력으로 조작)
    public class Player : Entity
    {
        private ConsoleKeyInfo? _currentKeyInput;

        public Player(int x, int y, Renderer renderer) : base(x, y, "O", renderer)
        {
        }

        // GameManager에서 매 프레임 이번에 들어온 입력값을 전달받음
        public void SetInput(ConsoleKeyInfo? keyInput)
        {
            _currentKeyInput = keyInput;
        }

        public override void Update()
        {
            if (_currentKeyInput.HasValue)
            {
                switch (_currentKeyInput.Value.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        Y = Math.Max(1, Y - 1);
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        Y = Math.Min(RendererRef.Height - 2, Y + 1);
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        X = Math.Max(1, X - 2);
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        X = Math.Min(RendererRef.Width - 3, X + 1);
                        break;
                }
                _currentKeyInput = null; // 처리 후 입력 초기화
            }
        }
    }

    // 3. 간단한 NPC 엔티티 예시 (자동으로 움직이거나 제자리 마크)
    public class Npc : Entity
    {
        private int _moveTimer = 0;

        public Npc(int x, int y, string symbol, Renderer renderer) : base(x, y, symbol, renderer)
        {
        }

        public override void Update()
        {
            _moveTimer++;
            // 예를 들어 12프레임(약 1초)마다 좌우로 슥슥 움직이는 로직 등
            if (_moveTimer % 24 == 0)
            {
                // 간단한 AI 행동 예시
                X = X + 1;
            }
            else if (_moveTimer % 24 == 12)
            {
                X = X - 1;
            }
        }
    }
}
