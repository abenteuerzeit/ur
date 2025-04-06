# The Royal Game of UR Console Game

### TODO

### UI

- [x] Game board 
- [ ] Add, move, and remove player pieces.
- [ ] Simulate dice rolls per distribtuin 
- [ ] Score board. Track pieces of each player. 

### Gameplay

- [ ] Implement game rules. 
- [ ] Implement player paths 
- [ ] Human vs Human
- [ ] Human vs Computer
- [ ] Human vs AI

#### Rules

Source: <https://gdkeys.com/the-design-of-the-royal-game-of-ur/>

1. Each player owns `7` pieces.
2. Each piece must **first enter the board**, then **travel all the way until the end of the path**, and finally **exit by rolling the exact number** to jump out the board (`last cell +1`).
3. Players will throw `4` dice that can result either in a `0` or a `1`. The original version uses `4` sided dice that have `2` points painted in black and `2` in white.
4. Once the dice are thrown, a number between `0` and `4` will be obtained. This number can be **used to move any piece** by this exact number. Subdividing the number isn't possible. Example: I roll a `3`, I could bring **a new piece** on the table to the third position, **advance any one piece** by 3 squares, or make a piece **exit the board** if the third move makes it exit the board. If no move is possible, then **the turn is lost**. Also, moving a piece is an **obligation** if possible, which shouldn't be a surprise for Backgammon players.
5. The first `4` squares, and the last `2`, are safe squares for both players (only they can have pieces there) but the `8` central ones, the *Combat* ones, are shared between players, thus putting pieces at risk.
6. If a player moves one of their pieces on a square already occupied by one of their opponent's pieces, the enemy piece is "eaten" and goes back at the start, outside the board.
7. The `5` Rosace squares are *Jokers*: landing a piece on one gives **an extra turn** to the player, but that's not all: a piece sitting on a Rosace **cannot be eaten** (applicable, of course, only to the center Rosace, making it effectively the best cell of the game).