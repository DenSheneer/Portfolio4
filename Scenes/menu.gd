class_name MainMenu
extends Control


@onready var start_button: Button = $MarginContainer/HBoxContainer/VBoxContainer/StartButton as Button
@onready var quit_button: Button = $MarginContainer/HBoxContainer/VBoxContainer/QuitButton as Button
@onready var startlevel = preload("res://Scenes/main.tscn") as PackedScene

func _on_start_button_button_down() -> void:
	get_tree().change_scene_to_packed(startlevel)

func _on_quit_button_button_down() -> void:
	get_tree().quit()
