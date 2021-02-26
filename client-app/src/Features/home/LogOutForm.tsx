import React, { Fragment } from "react";
import { Button, Divider, Grid, Header, Segment } from "semantic-ui-react";
import { LoginForm } from "../../Features/user/LoginForm";
import { RegisterForm } from "../../Features/user/RegisterForm";
import ModalStore from "../../App/stores/ModalStore";

export const LogOutForm = () => {
  const { openModal } = ModalStore;
  return (
    <Fragment>
      <Header color="black" as="h3" content="Wanna try something new?" />
      <Segment placeholder>
        <Grid columns={2} relaxed="very" stackable>
          <Grid.Column>
            <LoginForm />
          </Grid.Column>

          <Grid.Column verticalAlign="middle">
            <Button
              onClick={() => openModal(<RegisterForm />)}
              content="Sign up"
              icon="signup"
              size="big"
            />
          </Grid.Column>
        </Grid>

        <Divider vertical>Or</Divider>
      </Segment>
    </Fragment>
  );
};
