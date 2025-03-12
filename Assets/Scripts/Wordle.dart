String GameWord;
String[] wordBank ={
  "brain",
  "tacos",
  "audio",
  "plain",
  "trees",
  "smoke",
  "cakes",
  "phone",
  "serve",
  "count",
  "paper",
  "water",
  "salty",
  "create"
};
int rows = 6;
int cols = 5;
char[][] guesses = new char[rows][cols];
for (int i = 0; i < rows; i++) {
  for (int j = 0; j < cols; j++) {
    guesses[i][j] = ' ';
  }
}
int gridSize = 100;
int currentRow = 0;
int currentCol = 0;

String typing="";
void setup(){
  size(cols * gridSize,rows * gridSize);
}

void draw(){
  fill(0);
  stroke(255);
  // making the grid and chars
  for(int r = 0; r < rows; r++){
    for(int c=0; c < cols; c++){
      rect(c * gridSize, r * gridSize, gridSize, gridSize);
      fill(255);
      textSize(32);
      textAlign(CENTER, CENTER);
      if (guesses[r][c] != ' ') {
        text(guesses[r][c], c * gridSize + gridSize / 2, r * gridSize + gridSize / 2);
      }
    }
  }
  textSize(32);
  text("Guess The Word",width/2-100,90);
}

void keyPressed() {
  if (key == ENTER) {
    if (currentCol == cols) {
      checkWord();
    }
  } else if (key == BACKSPACE) {
    if (currentCol > 0) {
      guesses[currentRow][currentCol] = '';
      currentCol--;
    }
  } else if (Character.isLetter(key) && currentCol < cols) {
    guesses[currentRow][currentCol] = Character.toLowerCase(key);
    currentCol++;
  }
}

void checkWord() {}