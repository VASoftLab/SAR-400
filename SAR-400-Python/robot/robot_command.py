class RobotCommand:

    def __init__(self, command_duration, command_joints=[]):
        self.command_duration = command_duration
        self.command_joints = command_joints

    def __repr__(self):
        return "CommandDuration:" + str(self.command_duration) + \
               "\n\tCommand_joints:" + "\n".join(repr(command_joint) for command_joint in self.command_joints)

    class CommandJoint:

        def __init__(self, joint_name, joint_value):
            self.joint_name = joint_name
            self.joint_value = joint_value

        def __repr__(self):
            return "JointName:" + self.joint_name + \
                   "\tJointValue:" + str(self.joint_value)
