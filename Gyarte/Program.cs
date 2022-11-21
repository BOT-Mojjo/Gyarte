using System;
using System.Threading;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using Raylib_cs;

double time = 0;
List<double> times = new List<double>(); 
bool click = false;
bool timer = false;
bool debug = false;
Random gen = new Random();
int tFPS = Raylib.GetMonitorRefreshRate(Raylib.GetCurrentMonitor());
if(tFPS == 0) tFPS = 60;
int phase = 0;
var Stopwatch = new Stopwatch();
int cd = gen.Next(tFPS*3, tFPS*5);
double avgReactionTime = 0;
int rCd = 0;
bool choice = true;
int tempCD = 2*tFPS;

Color foreground = new Color(255,255,255,255);
Color background = new Color(0,0,0,255);
System.Numerics.Vector2 vectorCenter = new System.Numerics.Vector2(500f, 600f);
Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
// Raylib.InitWindow(Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), "Test1");
Raylib.InitWindow(1280,720,"test2");
Raylib.SetTargetFPS(tFPS);

while(Raylib.WindowShouldClose() == false && phase == 0){
    int textsize = Raylib.GetScreenHeight()/30;
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.BLACK);

    Raylib.DrawText("When the program starts it will wait untill the user presses M1 to indicate they are ready.", 20, 20+(textsize*0), textsize, Color.WHITE);
    Raylib.DrawText("When they have, the program will wait 3-5 seconds and then the screen will flash white ", 20, 20+(textsize*1), textsize, Color.WHITE);
    Raylib.DrawText("and the user should press M1 as fast as they can. This will repeat 10 times.", 20, 20+(textsize*2), textsize, Color.WHITE);
    Raylib.DrawText("After which the program will wait for the user to be ready, again by wating for a M1 input.", 20, 20+(textsize*4), textsize, Color.WHITE);
    Raylib.DrawText("Then it will briefly flash a shape in the middle of the screen", 20, 20+(textsize*5), textsize, Color.WHITE);
    Raylib.DrawText("which the user will then have to point out in a lineup of different shapes:", 20, 20+(textsize*6), textsize, Color.WHITE);
    Raylib.DrawText("a triangle, a square, a pentagon, a hexagon, and a circle, by clicking on them.", 20, 20+(textsize*7), textsize, Color.WHITE);
    Raylib.DrawText("This will repeat until both the colorValue difference and the time it flashes on screen ", 20, 20+(textsize*8), textsize, Color.WHITE);
    Raylib.DrawText("has reached near-zero.", 20, 20+(textsize*9), textsize, Color.WHITE);
    Raylib.DrawText("Press Mouse Button 1, M1, to start.", 20, 20+(textsize*11), textsize, Color.WHITE);

    if(Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) phase++;

    Raylib.EndDrawing();
}

Stopwatch.Start();
while(Raylib.WindowShouldClose() == false && phase == 1){  //Phase 1, e.i. testing the reaction time of the subject.
    Raylib.BeginDrawing();                          //Frame clear setup
    Raylib.ClearBackground(Color.BLACK);
    if(!choice){
        if(tempCD > 0){
            tempCD--;
        } else {
            choice = Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON);
        }
    } else {

    if(!click){  //click check, kinda redundant but needed to start the sequence
        click = Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON);
    }
    

    if(click){
        if(cd!=0){
            cd--;
        } else {
            if(!Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON)) {       
                if(!timer){                     //Timer #2 using system.diagnostics
                    timer = true;
                    Stopwatch.Restart();
                }         
                Raylib.ClearBackground(Color.WHITE);
                time = time + Raylib.GetFrameTime();  //Timer #1 using Raylib, take the time between each frame, and add them all together to get the total time.
            } else if(time == 0){                     //to make it impossible to just hold m1 and cheat the reaction time test
                cd = gen.Next(tFPS*3, tFPS*5);
            } else {                                  //takes the time when the subject has reacted, and restarts the sequence
                times.Add(time);
                double tempTime = Stopwatch.ElapsedMilliseconds;
                tempTime = tempTime/1000;
                times.Add(tempTime);
                time=0;
                timer = false;
                cd = gen.Next(tFPS*3, tFPS*5);
            }
        }
    }

    }
    Debug();
    DebugSwitch();

    if (times.Count == 20){ //TODO MAKE 20 LATER
        Raylib.ClearBackground(Color.BLACK);
        phase = 2;
    }
    
    Raylib.EndDrawing();
}

click = false;
foreach(double i in times){
    avgReactionTime = avgReactionTime + i;
}
avgReactionTime = avgReactionTime/times.Count;
int repeats = 0;
List<double> timeGiven = new List<double>();
timeGiven.Add(avgReactionTime);
rCd = (int) (timeGiven[repeats] * tFPS)/2;
tempCD = 2*tFPS;
int tempCD2 = tFPS;
int shape = gen.Next(3,8);
List<int> choises = new List<int>();
List<Color> color = new List<Color>();
vectorCenter.X = (Raylib.GetScreenWidth()/2)-50;
vectorCenter.Y = (Raylib.GetScreenHeight()/2)-50;
while(Raylib.WindowShouldClose() == false && phase == 2){  //Phase 2 
    Raylib.BeginDrawing();
    Raylib.ClearBackground(background);
    
    if(!click && tempCD == 0){  //click check, kinda redundant but needed to start the sequence
        Raylib.DrawRectangle(0,20,20,20,Color.BLUE);
        click = Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON);
        choice = true;
        // changeConstant = colorConstant * repeats;
        // foreground = new Color(255-changeConstant,255-changeConstant,255-changeConstant,255);
        // // foreground = new Color(255-changeConstant,255-changeConstant,255-changeConstant,255);
        

        // if(repeats == 20){
        //     changeConstant = colorConstant * 7;
        //     background = new Color(0+changeConstant/3,0+changeConstant/3,0+changeConstant/3,255);
        //     // background = new Color(0+changeConstant/3,0+changeConstant/3,0+changeConstant/3,255);
        // }
    }
    

    if(click){
        if(cd!=0){
            cd--;
        } else if(cd == 0 && rCd > 0){
            if(shape == 7){  //because i don't like septagons 7 will now become a circle
                Raylib.DrawCircle((int) vectorCenter.X, (int) vectorCenter.Y, Raylib.GetScreenWidth()/5, foreground);
            } else {
                Raylib.DrawPoly(vectorCenter, shape, Raylib.GetScreenWidth()/5, 0, foreground);
            }
            // Raylib.DrawRectangle(centerObjx, centerObjy, 100, 100, foreground);
            rCd--;
        }
        if(rCd == 0){

            if(choice){
                if(tempCD2 == 0){
                    int temp2 = 0;
                    for(int i = 3; i < 8; i++){
                        temp2++;
                        if(i == 7){  //because i don't like septagons 7 will now become a circle
                            Raylib.DrawCircle((int) (Raylib.GetScreenWidth()/6)*temp2, (int) vectorCenter.Y, Raylib.GetScreenWidth()/20, Color.WHITE);
                        } else {
                            System.Numerics.Vector2 tempVector = new System.Numerics.Vector2((Raylib.GetScreenWidth()/6)*temp2, vectorCenter.Y);
                            Raylib.DrawPoly(tempVector, i, Raylib.GetScreenWidth()/20, 0, Color.WHITE);
                        }
                    }
                    // Raylib.DrawText(Raylib.GetMouseX().ToString(), 0, 20, 15, Color.GREEN);
                    if(Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON)){
                        int temp3 = Raylib.GetMouseX();
                        int choiceX = (Raylib.GetScreenWidth()/6);
                        int choiceWidth = (Raylib.GetScreenWidth()/40);
                        if(temp3 > choiceX - choiceWidth && temp3 < choiceX + choiceWidth){
                            choises.Add(3);
                            choice = false;
                        }
                        if(temp3 > choiceX*2 - choiceWidth && temp3 < choiceX*2 + choiceWidth){
                            choises.Add(4);
                            choice = false;
                        }
                        if(temp3 > choiceX*3 - choiceWidth && temp3 < choiceX*3 + choiceWidth){
                            choises.Add(5);
                            choice = false;
                        }
                        if(temp3 > choiceX*4 - choiceWidth && temp3 < choiceX*4 + choiceWidth){
                            choises.Add(6);
                            choice = false;
                        }
                        if(temp3 > choiceX*5 - choiceWidth && temp3 < choiceX*5 + choiceWidth){
                            choises.Add(7);
                            choice = false;
                        }

                    }
                } else {
                    tempCD2--;
                }
            } else {

                click = false;
                rCd = (int) (timeGiven[repeats] * tFPS)/2;
                // cd = gen.Next(tFPS*3, tFPS*5);
                cd = 30;
                SetColor();
                choises.Add(shape);
                shape = gen.Next(3,8);
                repeats++;
                tempCD2 = tFPS;
                if(rCd == 0) phase++;

            }
        }
    }
    Debug();
    DebugSwitch();
    Raylib.EndDrawing();

    if(tempCD > 0){
        tempCD--;
    }
}

subject Teste = new subject();
Teste.AvgReactionTime = avgReactionTime;
Teste.PreReactionTimes = times;
Teste.PostReactionTimes = timeGiven;
Teste.Choises = choises;
Teste.LengthOfTest = color.Count;
List<(int, int, int, int)> tempColorList = new List<(int, int, int, int)>();
foreach(Color temp in color){
    (int, int, int, int) tempII = (temp.r, temp.g, temp.b, temp.a); 
    tempColorList.Add(tempII);
}

// foreach((int, int, int, int) value in tempColorList){ //reduce redundence in output Json file
//     Teste.Color.Add(value.Item1);
// }

// Teste.Color = tempColorList;
var options = new JsonSerializerOptions
{
    IncludeFields = true,
};
try{
    string export = JsonSerializer.Serialize<subject>(Teste, options);
    File.WriteAllText("gyarteSubject" + DateTime.Now.Ticks.ToString() + ".json", export);
} catch {

}



void Debug(){
    if(debug){  //so you can view normally hidden values without pausing the program.
        Raylib.DrawFPS(0,0);
        int tempX = (Raylib.GetScreenWidth()/4)*3;
        
        Raylib.DrawText(rCd.ToString() + " " + avgReactionTime, tempX, 0, 15, Color.GREEN);
        for(int i = 0; i < times.Count; i++){
            Raylib.DrawText(times[i].ToString(),tempX,(i+1)*20, 15, Color.GREEN);

        }
        Raylib.DrawText(cd.ToString(), 0, 20, 15, Color.GREEN);
        if(phase > 0){
            Raylib.DrawText(background.r.ToString(), 0, 60, 15, Color.GREEN);
            Raylib.DrawText(foreground.r.ToString(), 0, 40, 15, Color.GREEN);
        }
    }
}

void DebugSwitch(){
    if(Raylib.IsKeyPressed(KeyboardKey.KEY_M)) debug = true;
    if(Raylib.IsKeyPressed(KeyboardKey.KEY_N)) debug = false;
}

void SetColor(){
    color.Add(foreground);
    color.Add(background);
    if(repeats < 24){
        if(foreground.r - background.r > 125){
            foreground.r -= 15;
            background.r += 5;
            foreground.g -= 15;
            background.g += 5;
            foreground.b -= 15;
            background.b += 5;
        } else if (foreground.r - background.r > 25){
            foreground.r -= 10;
            background.r += 2;
            foreground.g -= 10;
            background.g += 2;
            foreground.b -= 10;
            background.b += 2;
        } else if (foreground.r != background.r){
            foreground.r--;
            background.r++;
            foreground.g--;
            background.g++;
            foreground.b--;
            background.b++;
        }
        timeGiven.Add(timeGiven[repeats]);
    } else {
        timeGiven.Add((timeGiven[repeats]/3)*2);
    }
    
}