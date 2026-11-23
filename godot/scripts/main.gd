extends Node3D

var player: CharacterBody3D
var camera_pivot: Node3D
var yaw := 0.0
var pitch := -0.28
var touch_look_id := -1
var last_touch := Vector2.ZERO
var move_buttons := {"forward": false, "back": false, "left": false, "right": false}
var jump_requested := false
var best_height := 0.0
var hud_label: Label

const SPEED := 6.2
const JUMP_VELOCITY := 8.4
const GRAVITY := 22.0

func _ready() -> void:
	_build_world()
	_build_player()
	_build_ui()

func _build_world() -> void:
	var world_env := WorldEnvironment.new()
	var env := Environment.new()
	env.background_mode = Environment.BG_SKY
	var sky := Sky.new()
	var sky_mat := ProceduralSkyMaterial.new()
	sky_mat.sky_top_color = Color(0.12, 0.34, 0.62)
	sky_mat.sky_horizon_color = Color(0.65, 0.82, 0.94)
	sky.sky_material = sky_mat
	env.sky = sky
	env.ambient_light_source = Environment.AMBIENT_SOURCE_SKY
	world_env.environment = env
	add_child(world_env)
	var sun := DirectionalLight3D.new()
	sun.rotation_degrees = Vector3(-48, -32, 0)
	sun.light_energy = 1.35
	add_child(sun)
	_make_platform(Vector3(0, -0.35, 0), Vector3(12, 0.7, 12), Color(0.20, 0.28, 0.32))
	var colors := [Color(0.24,0.55,0.72), Color(0.31,0.62,0.48), Color(0.75,0.48,0.25), Color(0.53,0.42,0.72)]
	for i in range(1, 58):
		var angle := float(i) * 1.13
		var radius := 2.6 + float(i % 4) * 0.32
		var pos := Vector3(cos(angle) * radius, float(i) * 1.72, sin(angle) * radius)
		var size := Vector3(3.2 if i % 6 != 0 else 2.5, 0.48, 3.2 if i % 6 != 0 else 2.5)
		_make_platform(pos, size, colors[i % colors.size()])
		if i % 8 == 0:
			_make_checkpoint(pos + Vector3(0, 1.0, 0))

func _make_platform(pos: Vector3, size: Vector3, color: Color) -> void:
	var body := StaticBody3D.new()
	body.position = pos
	var mesh := MeshInstance3D.new()
	var box := BoxMesh.new()
	box.size = size
	mesh.mesh = box
	var mat := StandardMaterial3D.new()
	mat.albedo_color = color
	mat.roughness = 0.82
	mesh.material_override = mat
	body.add_child(mesh)
	var collision := CollisionShape3D.new()
	var shape := BoxShape3D.new()
	shape.size = size
	collision.shape = shape
	body.add_child(collision)
	add_child(body)

func _make_checkpoint(pos: Vector3) -> void:
	var marker := MeshInstance3D.new()
	var mesh := CylinderMesh.new()
	mesh.top_radius = 0.12
	mesh.bottom_radius = 0.12
	mesh.height = 2.0
	marker.mesh = mesh
	marker.position = pos
	var mat := StandardMaterial3D.new()
	mat.albedo_color = Color(1.0, 0.78, 0.12)
	mat.emission_enabled = true
	mat.emission = Color(0.7, 0.38, 0.03)
	marker.material_override = mat
	add_child(marker)

func _build_player() -> void:
	player = CharacterBody3D.new()
	player.position = Vector3(0, 1.1, 0)
	var mesh := MeshInstance3D.new()
	var capsule := CapsuleMesh.new()
	capsule.radius = 0.38
	capsule.height = 1.75
	mesh.mesh = capsule
	var mat := StandardMaterial3D.new()
	mat.albedo_color = Color(0.95, 0.39, 0.15)
	mesh.material_override = mat
	player.add_child(mesh)
	var collision := CollisionShape3D.new()
	var shape := CapsuleShape3D.new()
	shape.radius = 0.38
	shape.height = 1.75
	collision.shape = shape
	player.add_child(collision)
	add_child(player)
	camera_pivot = Node3D.new()
	camera_pivot.position = Vector3(0, 1.0, 0)
	player.add_child(camera_pivot)
	var camera := Camera3D.new()
	camera.position = Vector3(0, 2.4, 7.0)
	camera.fov = 72
	camera_pivot.add_child(camera)
	camera.current = true

func _build_ui() -> void:
	var layer := CanvasLayer.new()
	add_child(layer)
	var ui := Control.new()
	ui.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	layer.add_child(ui)
	hud_label = Label.new()
	hud_label.text = "ALTURA: 0 m"
	hud_label.position = Vector2(20, 18)
	hud_label.add_theme_font_size_override("font_size", 24)
	hud_label.add_theme_color_override("font_color", Color.WHITE)
	ui.add_child(hud_label)
	_add_control_button(ui, "↑", Vector2(38, -170), "forward")
	_add_control_button(ui, "↓", Vector2(38, -78), "back")
	_add_control_button(ui, "←", Vector2(130, -78), "left")
	_add_control_button(ui, "→", Vector2(222, -78), "right")
	_add_control_button(ui, "PULAR", Vector2(-150, -115), "jump")
	var hint := Label.new()
	hint.text = "Arraste o lado direito para olhar"
	hint.position = Vector2(20, 54)
	hint.add_theme_font_size_override("font_size", 15)
	hint.add_theme_color_override("font_color", Color(1,1,1,0.8))
	ui.add_child(hint)

func _add_control_button(parent: Control, caption: String, offset: Vector2, action: String) -> void:
	var button := Button.new()
	button.text = caption
	button.custom_minimum_size = Vector2(78, 72) if action != "jump" else Vector2(112, 100)
	button.anchor_left = 0.0 if offset.x >= 0 else 1.0
	button.anchor_right = button.anchor_left
	button.anchor_top = 1.0
	button.anchor_bottom = 1.0
	button.position = offset
	button.add_theme_font_size_override("font_size", 24 if action != "jump" else 20)
	button.modulate = Color(1,1,1,0.88)
	parent.add_child(button)
	if action == "jump":
		button.button_down.connect(func(): jump_requested = true)
	else:
		button.button_down.connect(func(): move_buttons[action] = true)
		button.button_up.connect(func(): move_buttons[action] = false)

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		if event.pressed and event.position.x > get_viewport().get_visible_rect().size.x * 0.42:
			touch_look_id = event.index
			last_touch = event.position
		elif not event.pressed and event.index == touch_look_id:
			touch_look_id = -1
	elif event is InputEventScreenDrag and event.index == touch_look_id:
		var delta := event.position - last_touch
		last_touch = event.position
		yaw -= delta.x * 0.006
		pitch = clamp(pitch - delta.y * 0.004, -0.9, 0.35)
	elif event is InputEventMouseMotion and Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT):
		yaw -= event.relative.x * 0.006
		pitch = clamp(pitch - event.relative.y * 0.004, -0.9, 0.35)

func _physics_process(delta: float) -> void:
	if not player:
		return
	var input := Vector2.ZERO
	input.y -= float(move_buttons["forward"] or Input.is_action_pressed("move_forward"))
	input.y += float(move_buttons["back"] or Input.is_action_pressed("move_back"))
	input.x -= float(move_buttons["left"] or Input.is_action_pressed("move_left"))
	input.x += float(move_buttons["right"] or Input.is_action_pressed("move_right"))
	var direction := (Basis(Vector3.UP, yaw) * Vector3(input.x, 0, input.y)).normalized()
	player.velocity.x = direction.x * SPEED
	player.velocity.z = direction.z * SPEED
	if not player.is_on_floor():
		player.velocity.y -= GRAVITY * delta
	elif jump_requested or Input.is_action_just_pressed("jump"):
		player.velocity.y = JUMP_VELOCITY
	jump_requested = false
	player.move_and_slide()
	camera_pivot.rotation = Vector3(pitch, yaw, 0)
	if player.position.y < -8:
		player.position = Vector3(0, 2, 0)
		player.velocity = Vector3.ZERO
	best_height = max(best_height, player.position.y)
	hud_label.text = "ALTURA: %d m" % int(best_height)
