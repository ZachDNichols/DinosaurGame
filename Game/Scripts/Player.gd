extends CharacterBody2D

class_name Player

enum Direction{
	Up,
	Left,
	Right,
	Down,
}

const SPEED = 100.0
var direction = Direction.Down

func _physics_process(delta):
	movePlayer()

func movePlayer():
	var horizontal = Input.get_axis("ui_left", "ui_right")
	var vertical = Input.get_axis("ui_up", "ui_down")
	
	if horizontal:
		velocity.x = horizontal * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		
	if vertical:
		velocity.y = vertical * SPEED
	else:
		velocity.y = move_toward(velocity.y, 0, SPEED)

	velocity = velocity.normalized() * SPEED
	
	setDirection(velocity)
	print(Direction.keys()[direction])
	
	move_and_slide()

func setDirection(velocity):
	if velocity.y > 0 and velocity.x == 0:
		direction = Direction.Down
	elif velocity.y < 0 and velocity.x == 0:
		direction = Direction.Up
	elif velocity.y == 0 and velocity.x > 0:
		direction = Direction.Right
	elif velocity.y == 0 and velocity.x < 0:
		direction = Direction.Left
